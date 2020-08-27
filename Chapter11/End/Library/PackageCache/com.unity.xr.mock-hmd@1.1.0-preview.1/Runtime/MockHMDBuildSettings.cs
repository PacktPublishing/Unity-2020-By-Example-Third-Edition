using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

namespace Unity.XR.MockHMD
{
    /// <summary>
    /// Build-time settings for MockHMD provider.
    /// </summary>
    [XRConfigurationData("MockHMD", MockHMDBuildSettings.BuildSettingsKey)]
    public class MockHMDBuildSettings : ScriptableObject
    {
        public const string BuildSettingsKey = "xr.sdk.mock-hmd.settings";

        /// <summary>
        /// Stereo rendering mode.
        /// </summary>
        public enum RenderMode
        {
            /// <summary>
            /// Submit separate draw calls for each eye.
            /// </summary>
            MultiPass,

            /// <summary>
            /// Submit one draw call for both eyes.
            /// </summary>
            SinglePassInstanced,
        };

        /// <summary>
        /// Stereo rendering mode.
        /// </summary>
        public RenderMode renderMode;

        /// <summary>
        /// Runtime access to build settings.
        /// </summary>
        public static MockHMDBuildSettings Instance
        {
            get
            {
                MockHMDBuildSettings settings = null;
#if UNITY_EDITOR
                UnityEngine.Object obj = null;
                UnityEditor.EditorBuildSettings.TryGetConfigObject(BuildSettingsKey, out obj);
                if (obj == null || !(obj is MockHMDBuildSettings))
                    return null;
                settings = (MockHMDBuildSettings) obj;
#else
                settings = s_RuntimeInstance;
                if (settings == null)
                    settings = new MockHMDBuildSettings();
#endif
                return settings;
            }
        }

#if !UNITY_EDITOR
        /// <summary>Static instance that will hold the runtime asset instance we created in our build process.</summary>
        public static MockHMDBuildSettings s_RuntimeInstance = null;

        void OnEnable()
        {
            s_RuntimeInstance = this;
        }
#endif
    }
}