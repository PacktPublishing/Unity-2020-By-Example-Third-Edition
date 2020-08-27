using System.Runtime.InteropServices;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// An internal class with only static methods to register the environment probe subsystem before the scene is
    /// loaded.
    /// </summary>
    static class ARCoreEnvironmentProbeRegistration
    {
        /// <summary>
        /// Create and register the environment probe subsystem descriptor to advertise a providing implementation for
        /// environment probe functionality.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            const string subsystemId = "ARCore-EnvironmentProbe";
            XREnvironmentProbeSubsystemCinfo environmentProbeSubsystemInfo = new XREnvironmentProbeSubsystemCinfo()
            {
                id = subsystemId,
                implementationType = typeof(ARCoreEnvironmentProbeSubsystem),
                supportsManualPlacement = false,
                supportsRemovalOfManual = false,
                supportsAutomaticPlacement = true,
                supportsRemovalOfAutomatic = false,
                supportsEnvironmentTexture = false,
                supportsEnvironmentTextureHDR = true,
            };

            if (!XREnvironmentProbeSubsystem.Register(environmentProbeSubsystemInfo))
            {
                Debug.LogErrorFormat("Cannot register the {0} subsystem", subsystemId);
            }
#endif
        }
    }

    /// <summary>
    /// This subsystem provides implementing functionality for the <c>XREnvironmentProbeSubsystem</c> class.
    /// </summary>
    [Preserve]
    class ARCoreEnvironmentProbeSubsystem : XREnvironmentProbeSubsystem
    {
        protected override Provider CreateProvider() => new ARCoreProvider();

        class ARCoreProvider : Provider
        {
            public ARCoreProvider() => NativeApi.UnityARCore_EnvironmentProbeProvider_Construct();

            /// <summary>
            /// Starts the environment probe subsystem by enabling the HDR Environmental Light Estimation.
            /// </summary>
            public override void Start() => NativeApi.UnityARCore_EnvironmentProbeProvider_Start();

            /// <summary>
            /// Stops the environment probe subsystem by disabling the environment probe state.
            /// </summary>
            public override void Stop() => NativeApi.UnityARCore_EnvironmentProbeProvider_Stop();

            /// <summary>
            /// Destroy the environment probe subsystem by first ensuring that the subsystem has been stopped and then
            /// destroying the provider.
            /// </summary>
            public override void Destroy() => NativeApi.UnityARCore_EnvironmentProbeProvider_Destroy();

            /// <summary>
            /// Enable or disable automatic placement of environment probes by the provider.
            /// </summary>
            /// <param name='value'><c>true</c> if the provider should automatically place environment probes in the scene.
            /// Otherwise, <c>false</c></param>.
            /// <remarks>ARCore does not allow Environment Probes to be placed manually.  Regardless of value this will always be automatic.</remarks>
            public override void SetAutomaticPlacement(bool value)
            {
                if (!value)
                    throw new NotSupportedException("ARCore only supports the automatic placement of environment probes.");
            }

            /// <summary>
            /// Set the state of HDR environment texture generation.
            /// </summary>
            /// <param name="value">Whether HDR environment texture generation is enabled (<c>true</c>) or disabled
            /// (<c>false</c>).</param>
            /// <returns>
            /// Whether the HDR environment texture generation state was set.
            /// </returns>
            /// <remarks>ARCore will only ever return environmental textures that are HDR.  This can only be set to <c>true</c>.</remarks>
            public override bool TrySetEnvironmentTextureHDREnabled(bool value)
            {
                if (value)
                {
                    return true;
                }
                else
                {
                    throw new NotSupportedException("ARCore only supports HDR for environment textures.  Attempting to turn it off will not work.");
                }
            }

            public override TrackableChanges<XREnvironmentProbe> GetChanges(XREnvironmentProbe defaultEnvironmentProbe,
                                                                            Allocator allocator)
            {
                XREnvironmentProbe probe = XREnvironmentProbe.defaultValue;
                NativeApi.UnityARCore_EnvironmentProbeProvider_GetChanges(out int numAdded, out int numUpdated, out int numRemoved, ref probe);

                // There is only ever one probe currently so allocating using it as the default template is safe.
                var changes = new TrackableChanges<XREnvironmentProbe>(numAdded, numUpdated, numRemoved, allocator, probe);

                if (numRemoved > 0)
                {
                    var nativeRemovedArray = changes.removed;
                    nativeRemovedArray[0] = probe.trackableId;
                }

                return changes;
            }
        }
    }

    /// <summary>
    /// Container to wrap the native ARCore Environment Probe APIs.
    /// </summary>
    static class NativeApi
    {
        [DllImport("UnityARCore")]
        internal static extern void UnityARCore_EnvironmentProbeProvider_Construct();

        [DllImport("UnityARCore")]
        internal static extern void UnityARCore_EnvironmentProbeProvider_Start();

        [DllImport("UnityARCore")]
        internal static extern void UnityARCore_EnvironmentProbeProvider_Stop();

        [DllImport("UnityARCore")]
        internal static extern void UnityARCore_EnvironmentProbeProvider_Destroy();

        [DllImport("UnityARCore")]
        internal static extern void UnityARCore_EnvironmentProbeProvider_SetAutomaticPlacementEnabled();

        [DllImport("UnityARCore")]
        internal static extern void UnityARCore_EnvironmentProbeProvider_TrySetEnvironmentTextureHDREnabled();

        [DllImport("UnityARCore")]
        internal static extern void UnityARCore_EnvironmentProbeProvider_GetChanges(out int numAdded, out int numUpdated, out int numRemoved, ref XREnvironmentProbe outProbe);
    }
}
