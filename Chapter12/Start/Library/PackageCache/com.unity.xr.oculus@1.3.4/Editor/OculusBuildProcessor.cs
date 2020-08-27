using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Unity.XR.Oculus;
using UnityEditor;
using UnityEditor.XR.Management;
using UnityEngine.XR.Management;

namespace UnityEditor.XR.Oculus
{
    public class OculusBuildProcessor : XRBuildHelper<OculusSettings>
    {
        public override string BuildSettingsKey { get {return "Unity.XR.Oculus.Settings";} }
    }

    public static class OculusBuildTools
    {
        public static bool OculusLoaderPresentInSettingsForBuildTarget(BuildTargetGroup btg)
        {
            var generalSettingsForBuildTarget = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(btg);
            if (!generalSettingsForBuildTarget)
                return false;
            var settings = generalSettingsForBuildTarget.AssignedSettings;
            if (!settings)
                return false;
            List<XRLoader> loaders = settings.loaders;
            return loaders.Exists(loader => loader is OculusLoader);
        }

        public static OculusSettings GetSettings()
        {
            OculusSettings settings = null;
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject<OculusSettings>("Unity.XR.Oculus.Settings", out settings);
#else
            settings = OculusSettings.s_Settings;
#endif
            return settings;
        }
    }

    [InitializeOnLoad]
    public static class OculusEnterPlayModeSettingsCheck
    {
        static OculusEnterPlayModeSettingsCheck()
        {
            EditorApplication.playModeStateChanged += PlaymodeStateChangedEvent;
        }

        private static void PlaymodeStateChangedEvent(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                if (!OculusBuildTools.OculusLoaderPresentInSettingsForBuildTarget(BuildTargetGroup.Standalone))
                {
                    return;
                }

                if (PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows)[0] !=
                    GraphicsDeviceType.Direct3D11)
                {
                    Debug.LogError("D3D11 is currently the only graphics API compatible with the Oculus XR Plugin on desktop platforms. Please change the preferred Graphics API setting in Player Settings.");
                }
            }
        }
    }

    internal class OculusPrebuildSettings : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            if(!OculusBuildTools.OculusLoaderPresentInSettingsForBuildTarget(report.summary.platformGroup))
                return;

            if (report.summary.platformGroup == BuildTargetGroup.Android)
            {
                GraphicsDeviceType firstGfxType = PlayerSettings.GetGraphicsAPIs(EditorUserBuildSettings.activeBuildTarget)[0];
                if (firstGfxType != GraphicsDeviceType.OpenGLES3 && firstGfxType != GraphicsDeviceType.Vulkan && firstGfxType != GraphicsDeviceType.OpenGLES2)
                {
                    throw new BuildFailedException("OpenGLES2, OpenGLES3, and Vulkan are currently the only graphics APIs compatible with the Oculus XR Plugin on mobile platforms.");
                }
                if (PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel19)
                {
                    throw new BuildFailedException("Minimum API must be set to 19 or higher for Oculus XR Plugin.");

                }
            }

            if (report.summary.platformGroup == BuildTargetGroup.Standalone)
            {
                if (PlayerSettings.GetGraphicsAPIs(EditorUserBuildSettings.activeBuildTarget)[0] !=
                    GraphicsDeviceType.Direct3D11)
                {
                    throw new BuildFailedException("D3D11 is currently the only graphics API compatible with the Oculus XR Plugin on desktop platforms. Please change the Graphics API setting in Player Settings.");
                }
            }
        }
    }

#if UNITY_ANDROID
    internal class OculusManifest : IPostGenerateGradleAndroidProject
    {
        static readonly string k_AndroidURI = "http://schemas.android.com/apk/res/android";

        static readonly string k_AndroidManifestPath = "/src/main/AndroidManifest.xml";

        void UpdateOrCreateAttributeInTag(XmlDocument doc, string parentPath, string tag, string name, string value)
        {
            var xmlNode = doc.SelectSingleNode(parentPath + "/" + tag);

            if (xmlNode != null)
            {
                ((XmlElement)xmlNode).SetAttribute(name, k_AndroidURI, value);
            }
        }

        void UpdateOrCreateNameValueElementsInTag(XmlDocument doc, string parentPath, string tag,
            string firstName, string firstValue, string secondName, string secondValue)
        {
            var xmlNodeList = doc.SelectNodes(parentPath + "/" + tag);

            foreach (XmlNode node in xmlNodeList)
            {
                var attributeList = ((XmlElement)node).Attributes;

                foreach (XmlAttribute attrib in attributeList)
                {
                    if (attrib.Value == firstValue)
                    {
                        XmlAttribute valueAttrib = attributeList[secondName, k_AndroidURI];
                        if (valueAttrib != null)
                        {
                            valueAttrib.Value = secondValue;
                        }
                        else
                        {
                            ((XmlElement)node).SetAttribute(secondName, k_AndroidURI, secondValue);
                        }
                        return;
                    }
                }
            }
            
            // Didn't find any attributes that matched, create both (or all three)
            XmlElement childElement = doc.CreateElement(tag);
            childElement.SetAttribute(firstName, k_AndroidURI, firstValue);
            childElement.SetAttribute(secondName, k_AndroidURI, secondValue);

            var xmlParentNode = doc.SelectSingleNode(parentPath);

            if (xmlParentNode != null)
            {
                xmlParentNode.AppendChild(childElement);
            }
        }

        // same as above, but don't create if the node already exists
        void CreateNameValueElementsInTag(XmlDocument doc, string parentPath, string tag,
            string firstName, string firstValue, string secondName, string secondValue, string thirdName=null, string thirdValue=null)
        {
            var xmlNodeList = doc.SelectNodes(parentPath + "/" + tag);

            // don't create if the firstValue matches
            foreach (XmlNode node in xmlNodeList)
            {
                foreach (XmlAttribute attrib in node.Attributes)
                {
                    if (attrib.Value == firstValue)
                    {
                        return;
                    }
                }
            }
            
            XmlElement childElement = doc.CreateElement(tag);
            childElement.SetAttribute(firstName, k_AndroidURI, firstValue);
            childElement.SetAttribute(secondName, k_AndroidURI, secondValue);

            if (thirdValue != null)
            {
                childElement.SetAttribute(thirdName, k_AndroidURI, thirdValue);
            }

            var xmlParentNode = doc.SelectSingleNode(parentPath);

            if (xmlParentNode != null)
            {
                xmlParentNode.AppendChild(childElement);
            }
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            if(!OculusBuildTools.OculusLoaderPresentInSettingsForBuildTarget(BuildTargetGroup.Android))
                return;

            var manifestPath = path + k_AndroidManifestPath;
            var manifestDoc = new XmlDocument();
            manifestDoc.Load(manifestPath);

            var sdkVersion = (int)PlayerSettings.Android.minSdkVersion;

            UpdateOrCreateAttributeInTag(manifestDoc, "/", "manifest", "installLocation", "auto");

            var nodePath = "/manifest/application";
            UpdateOrCreateNameValueElementsInTag(manifestDoc, nodePath, "meta-data", "name", "com.samsung.android.vr.application.mode", "value", "vr_only");

            nodePath = "/manifest/application";
            UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "screenOrientation", "landscape");
            UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "theme", "@android:style/Theme.Black.NoTitleBar.Fullscreen");

            var configChangesValue = "keyboard|keyboardHidden|navigation|orientation|screenLayout|screenSize|uiMode";
            configChangesValue = ((sdkVersion >= 24) ? configChangesValue + "|density" : configChangesValue);
            UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "configChanges", configChangesValue);

            if (sdkVersion >= 24)
            {
                UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "resizeableActivity", "false");
            }

            UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "launchMode", "singleTask");

            if (!OculusBuildTools.GetSettings() || OculusBuildTools.GetSettings().V2Signing)
            {
                nodePath = "/manifest";
                CreateNameValueElementsInTag(manifestDoc, nodePath, "uses-feature", "name", "android.hardware.vr.headtracking", "required", "true", "version", "1");
            }

            manifestDoc.Save(manifestPath);
        }

        public int callbackOrder { get { return 10000; } }

        void DebugPrint(XmlDocument doc)
        {
            var sw = new System.IO.StringWriter();
            var xw = XmlWriter.Create(sw);
            doc.Save(xw);
            Debug.Log(sw);
        }
    }
#endif
}
