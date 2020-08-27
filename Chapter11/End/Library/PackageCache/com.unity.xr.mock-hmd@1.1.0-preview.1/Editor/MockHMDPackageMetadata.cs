#if XR_MGMT_320
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;

namespace Unity.XR.MockHMD.Editor
{
    internal class MockHMDMetadata : IXRPackage
    {
        private class MockHMDPackageMetadata : IXRPackageMetadata
        {
            public string packageName => "MockHMD XR Plugin";
            public string packageId => "com.unity.xr.mock-hmd";
            public string settingsType => "Unity.XR.MockHMD.MockHMDBuildSettings";

            private static readonly List<IXRLoaderMetadata> s_LoaderMetadata = new List<IXRLoaderMetadata>() { new MockHMDLoaderMetadata() };
            public List<IXRLoaderMetadata> loaderMetadata => s_LoaderMetadata;
        }

        private class MockHMDLoaderMetadata : IXRLoaderMetadata
        {
            public string loaderName => "Mock HMD Loader";
            public string loaderType => "Unity.XR.MockHMD.MockHMDLoader";

            private static readonly List<BuildTargetGroup> s_SupportedBuildTargets = new List<BuildTargetGroup>()
            {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.Android,
            };
            public List<BuildTargetGroup> supportedBuildTargets => s_SupportedBuildTargets;
        }

        private static IXRPackageMetadata s_Metadata = new MockHMDPackageMetadata();
        public IXRPackageMetadata metadata => s_Metadata;

        public bool PopulateNewSettingsInstance(ScriptableObject obj)
        {
            var settings = obj as MockHMDBuildSettings;
            if (settings != null)
            {
                settings.renderMode = MockHMDBuildSettings.RenderMode.SinglePassInstanced;
                return true;
            }

            return false;
        }
    }
}
#endif