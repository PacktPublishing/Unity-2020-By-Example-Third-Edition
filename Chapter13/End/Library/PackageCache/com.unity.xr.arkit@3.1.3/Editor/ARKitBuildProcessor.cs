#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.Rendering;
using UnityEditor.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARKit;

using OSVersion = UnityEngine.XR.ARKit.OSVersion;

namespace UnityEditor.XR.ARKit
{
    internal class ARKitBuildProcessor
    {
        public static IEnumerable<T> AssetsOfType<T>() where T : UnityEngine.Object
        {
            foreach(var guid in AssetDatabase.FindAssets("t:" + typeof(T).Name))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                yield return AssetDatabase.LoadAssetAtPath<T>(path);
            }
        }

        class PostProcessor : IPostprocessBuildWithReport
        {
            public int callbackOrder { get { return 0; } }

            public void OnPostprocessBuild(BuildReport report)
            {
                if (report.summary.platform != BuildTarget.iOS)
                {
                    return;
                }

                BuildHelper.RemoveShaderFromProject(ARKitCameraSubsystem.backgroundShaderName);
                HandleARKitRequiredFlag(report.summary.outputPath);
            }

            static void HandleARKitRequiredFlag(string pathToBuiltProject)
            {
                var arkitSettings = ARKitSettings.GetOrCreateSettings();
                string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));
                PlistElementDict rootDict = plist.root;

                // Get or create array to manage device capabilities
                const string capsKey = "UIRequiredDeviceCapabilities";
                PlistElementArray capsArray;
                PlistElement pel;
                if (rootDict.values.TryGetValue(capsKey, out pel))
                {
                    capsArray = pel.AsArray();
                }
                else
                {
                    capsArray = rootDict.CreateArray(capsKey);
                }
                // Remove any existing "arkit" plist entries
                const string arkitStr = "arkit";
                capsArray.values.RemoveAll(x => arkitStr.Equals(x.AsString()));
                if (arkitSettings.requirement == ARKitSettings.Requirement.Required)
                {
                    // Add "arkit" plist entry
                    capsArray.AddString(arkitStr);
                }

                File.WriteAllText(plistPath, plist.WriteToString());
            }
        }

        class Preprocessor : IPreprocessBuildWithReport, IPreprocessShaders
        {
            // Magic value according to
            // https://docs.unity3d.com/ScriptReference/PlayerSettings.GetArchitecture.html
            // "0 - None, 1 - ARM64, 2 - Universal."
            const int k_TargetArchitectureArm64 = 1;
            const int k_TargetArchitectureUniversal = 2;

            void SelectStaticLib()
            {
                const string pluginPath = "Packages/com.unity.xr.arkit/Runtime/iOS";
                LibUtil.SelectPlugin(
                    PluginImporter.GetAtPath($"{pluginPath}/Xcode1000/UnityARKit.a") as PluginImporter,
                    PluginImporter.GetAtPath($"{pluginPath}/Xcode1100/UnityARKit.a") as PluginImporter);
            }

            public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
            {
                // Remove shader variants for the camera background shader that will fail compilation because of package dependencies.
                string backgroundShaderName = ARKitCameraSubsystem.backgroundShaderName;
                if (backgroundShaderName.Equals(shader.name))
                {
                    foreach (var backgroundShaderKeywordToNotCompile in ARKitCameraSubsystem.backgroundShaderKeywordsToNotCompile)
                    {
                        ShaderKeyword shaderKeywordToNotCompile = new ShaderKeyword(shader, backgroundShaderKeywordToNotCompile);

                        for (int i = (data.Count - 1); i >= 0; --i)
                        {
                            if (data[i].shaderKeywordSet.IsEnabled(shaderKeywordToNotCompile))
                            {
                                data.RemoveAt(i);
                            }
                        }
                    }
                }
            }

            public void OnPreprocessBuild(BuildReport report)
            {
                if (report.summary.platform != BuildTarget.iOS)
                    return;

                if (string.IsNullOrEmpty(PlayerSettings.iOS.cameraUsageDescription))
                    throw new BuildFailedException("ARKit requires a Camera Usage Description (Player Settings > iOS > Other Settings > Camera Usage Description)");

                SelectStaticLib();
                EnsureMetalIsFirstApi();

                if(ARKitSettings.GetOrCreateSettings().requirement == ARKitSettings.Requirement.Required)
                {
                    EnsureMinimumBuildTarget();
                    EnsureTargetArchitecturesAreSupported(report.summary.platformGroup);
                }
                else if (PlayerSettings.GetArchitecture(report.summary.platformGroup) == k_TargetArchitectureUniversal)
                {
                    EnsureOpenGLIsUsed();
                }
                BuildHelper.AddBackgroundShaderToProject(ARKitCameraSubsystem.backgroundShaderName);
            }

            void EnsureMinimumBuildTarget()
            {
                var userSetTargetVersion = OSVersion.Parse(PlayerSettings.iOS.targetOSVersionString);
                if (userSetTargetVersion < new OSVersion(11))
                {
                    throw new BuildFailedException("You have selected a minimum target iOS version of " + userSetTargetVersion + " and have the ARKit package installed.  "
                        + "ARKit requires at least iOS version 11.0 (See Player Settings > Other Settings > Target minimum iOS Version).");
                }

            }

            void EnsureTargetArchitecturesAreSupported(BuildTargetGroup buildTargetGroup)
            {

                if (PlayerSettings.GetArchitecture(buildTargetGroup) != k_TargetArchitectureArm64)
                    throw new BuildFailedException("ARKit XR Plugin only supports the ARM64 architecture. See Player Settings > Other Settings > Architecture.");

            }

            void EnsureMetalIsFirstApi()
            {
                var graphicsApis = PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS);
                if (graphicsApis.Length > 0)
                {
                    var graphicsApi = graphicsApis[0];
                    if (graphicsApi != GraphicsDeviceType.Metal)
                        throw new BuildFailedException($"You currently have {graphicsApi} at the top of the list of Graphics APis. However, Metal needs to be first in the list. (See Player Settings > Other Settings > Graphics APIs)");
                }

            }

            void EnsureOpenGLIsUsed()
            {
                var graphicsApis = PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS);
                if (graphicsApis.Length > 0)
                {
                    if(!graphicsApis.Contains(GraphicsDeviceType.OpenGLES2))
                        throw new BuildFailedException("To build for 'Universal' architecture, OpenGLES2 is needed. (See Player Settings > Other Settings > Graphics APIs)");
                }

            }

            public int callbackOrder { get { return 0; } }
        }
    }
}
#endif
