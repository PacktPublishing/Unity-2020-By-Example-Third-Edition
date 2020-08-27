#if XR_MGMT_GTE_320

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;

namespace Unity.XR.Oculus.Editor
{
    internal class OculusMetadata : IXRPackage
    {
        private class OculusPackageMetadata : IXRPackageMetadata
        {
            public string packageName => "Oculus XR Plugin";
            public string packageId => "com.unity.xr.oculus";
            public string settingsType => "Unity.XR.Oculus.OculusSettings";
            public List<IXRLoaderMetadata> loaderMetadata => s_LoaderMetadata;

            private readonly static List<IXRLoaderMetadata> s_LoaderMetadata = new List<IXRLoaderMetadata>() { new OculusLoaderMetadata() };
        }

        private class OculusLoaderMetadata : IXRLoaderMetadata
        {
            public string loaderName => "Oculus";
            public string loaderType => "Unity.XR.Oculus.OculusLoader";
            public List<BuildTargetGroup> supportedBuildTargets => s_SupportedBuildTargets;

            private readonly static List<BuildTargetGroup> s_SupportedBuildTargets = new List<BuildTargetGroup>()
            {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.Android
            };
        }

        private static IXRPackageMetadata s_Metadata = new OculusPackageMetadata();
        public IXRPackageMetadata metadata => s_Metadata;

        public bool PopulateNewSettingsInstance(ScriptableObject obj)
        {
            var settings = obj as OculusSettings;
            if (settings != null)
            {
                settings.m_StereoRenderingModeDesktop = OculusSettings.StereoRenderingModeDesktop.MultiPass;
                settings.m_StereoRenderingModeAndroid = OculusSettings.StereoRenderingModeAndroid.MultiPass;
                settings.SharedDepthBuffer = true;
                settings.DashSupport = true;
                settings.V2Signing = true;

                return true;
            }

            return false;
        }
    }
}

#endif // XR_MGMT_GTE_320
