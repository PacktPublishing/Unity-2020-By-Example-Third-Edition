using System.Runtime.InteropServices;

namespace Unity.XR.MockHMD
{
    /// <summary>
    /// Runtime scripting API for Mock HMD provider.
    /// </summary>
    public static class MockHMD
    {
        private const string LibraryName = "UnityMockHMD";

        /// <summary>
        /// Set the stereo rendering mode.
        /// </summary>
        /// <param name="renderMode">rendering mode</param>
        /// <returns>true if render mode successfully set</returns>
        [DllImport(LibraryName, EntryPoint = "NativeConfig_SetRenderMode")]
        public static extern bool SetRenderMode(MockHMDBuildSettings.RenderMode renderMode);
    }
}