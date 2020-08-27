using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Management;

[assembly: InternalsVisibleTo("Unity.XR.Management.EditorTests")]
namespace UnityEditor.XR.Management
{
    class XRGeneralBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport, IPostGenerateGradleAndroidProject
    {
        class PreInitInfo
        {
            public PreInitInfo(IXRLoaderPreInit loader, BuildTarget buildTarget, BuildTargetGroup buildTargetGroup)
            {
                this.loader = loader;
                this.buildTarget = buildTarget;
                this.buildTargetGroup = buildTargetGroup;
            }

            public IXRLoaderPreInit loader;
            public BuildTarget buildTarget;
            public BuildTargetGroup buildTargetGroup;
        }

        static private PreInitInfo preInitInfo = null;

        public int callbackOrder
        {
            get { return 0;  }
        }

        void CleanOldSettings()
        {
            BuildHelpers.CleanOldSettings<XRGeneralSettings>();
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            // Always remember to cleanup preloaded assets after build to make sure we don't
            // dirty later builds with assets that may not be needed or are out of date.
            CleanOldSettings();

            XRGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings == null)
                return;

            XRGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(report.summary.platformGroup);
            if (settings == null)
                return;

            // store off some info about the first loader in the list for PreInit boot.config purposes
            preInitInfo = null;
            XRManagerSettings loaderManager = settings.AssignedSettings;
            if (loaderManager != null)
            {
                List<XRLoader> loaders = loaderManager.loaders;
                if (loaders.Count >= 1)
                {
                    preInitInfo = new PreInitInfo(loaders[0] as IXRLoaderPreInit, report.summary.platform, report.summary.platformGroup);
                }
            }

            if (loaderManager != null)
            {
                // chances are that our devices won't fall back to graphics device types later in the list so it's better to assume the device will be created with the first gfx api in the list.
                // furthermore, we have no way to influence falling back to other graphics API types unless we automatically change settings underneath the user which is no good!
                GraphicsDeviceType[] deviceTypes = PlayerSettings.GetGraphicsAPIs(report.summary.platform);
                if (deviceTypes.Length > 0)
                {
                    VerifyGraphicsAPICompatibility(loaderManager, deviceTypes[0]);
                }
                else
                {
                    Debug.LogWarning("No Graphics APIs have been configured in Player Settings.");
                }
            }

            UnityEngine.Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();

            if (!preloadedAssets.Contains(settings))
            {
                var assets = preloadedAssets.ToList();
                assets.Add(settings);
                PlayerSettings.SetPreloadedAssets(assets.ToArray());
            }
        }

        public static void VerifyGraphicsAPICompatibility(XRManagerSettings loaderManager, GraphicsDeviceType selectedDeviceType)
        {
                HashSet<GraphicsDeviceType> allLoaderGraphicsDeviceTypes = new HashSet<GraphicsDeviceType>();
                foreach (var loader in loaderManager.loaders)
                {
                    List<GraphicsDeviceType> supporteDeviceTypes = loader.GetSupportedGraphicsDeviceTypes(true);
                    // To help with backward compatibility, if we find that any of the compatibility lists are empty we assume that at least one of the loaders does not implement the GetSupportedGraphicsDeviceTypes method
                    // Therefore we revert to the previous behavior of building the app regardless of gfx api settings.
                    if (supporteDeviceTypes.Count == 0)
                    {
                        allLoaderGraphicsDeviceTypes.Clear();
                        break;
                    }
                    foreach (var supportedGraphicsDeviceType in supporteDeviceTypes)
                    {
                        allLoaderGraphicsDeviceTypes.Add(supportedGraphicsDeviceType);
                    }
                }


                if (allLoaderGraphicsDeviceTypes.Count > 0 && !allLoaderGraphicsDeviceTypes.Contains(selectedDeviceType))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendFormat(
                            "The selected grpahics API, {0}, is not supported by any of the current loaders. Please change the preferred Graphics API setting in Player Settings.\n",
                            selectedDeviceType);

                    foreach (var loader in loaderManager.loaders)
                    {
                        stringBuilder.AppendLine(loader.name + " supports:");
                        foreach (var supportedGraphicsDeviceType in loader.GetSupportedGraphicsDeviceTypes(true))
                        {
                            stringBuilder.AppendLine("\t -" + supportedGraphicsDeviceType);
                        }
                    }
                    throw new BuildFailedException(stringBuilder.ToString());
                }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            // Always remember to cleanup preloaded assets after build to make sure we don't
            // dirty later builds with assets that may not be needed or are out of date.
            CleanOldSettings();

            if (preInitInfo == null)
                return;

            // Android build post-processing is handled in OnPostGenerateGradleAndroidProject
            if (report.summary.platform != BuildTarget.Android)
            {
                foreach (BuildFile file in report.files)
                {
                    if (file.role == CommonRoles.bootConfig)
                    {
                        try
                        {
                            var loader = preInitInfo.loader;
                            if (loader != null)
                            {
                                string preInitLibraryName = loader.GetPreInitLibraryName(preInitInfo.buildTarget,
                                    preInitInfo.buildTargetGroup);
                                preInitInfo = null;
                                UnityEditor.XR.BootOptions.SetXRSDKPreInitLibrary(file.path,
                                    preInitLibraryName);
                            }
                        }
                        catch (Exception e)
                        {
                            throw new UnityEditor.Build.BuildFailedException(e);
                        }
                        break;
                    }
                }
            }
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            if (preInitInfo == null)
                return;

            // android builds move the files to a different location than is in the BuildReport, so we have to manually find the boot.config
            string[] paths = { "src", "main", "assets", "bin", "Data", "boot.config" };
            string fullPath = System.IO.Path.Combine(path, String.Join(Path.DirectorySeparatorChar.ToString(), paths));

            try
            {
                var loader = preInitInfo.loader;
                if (loader != null)
                {
                    string preInitLibraryName = loader.GetPreInitLibraryName(preInitInfo.buildTarget,
                        preInitInfo.buildTargetGroup);
                    preInitInfo = null;
                    UnityEditor.XR.BootOptions.SetXRSDKPreInitLibrary(fullPath, preInitLibraryName);
                }
            }
            catch (Exception e)
            {
                throw new UnityEditor.Build.BuildFailedException(e);
            }
        }
    }
}

