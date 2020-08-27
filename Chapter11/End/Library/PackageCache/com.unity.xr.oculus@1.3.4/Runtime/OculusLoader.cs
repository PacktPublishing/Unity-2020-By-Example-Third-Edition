using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Management;
using UnityEngine.XR;
#if UNITY_INPUT_SYSTEM 
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;
using Unity.XR.Oculus.Input;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Unity.XR.Oculus
{
#if UNITY_INPUT_SYSTEM
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    static class InputLayoutLoader
    {
        static InputLayoutLoader()
        {
            RegisterInputLayouts();
        }

        public static void RegisterInputLayouts()
        {
            InputSystem.RegisterLayout<OculusHMD>(
                matches: new InputDeviceMatcher()
                    .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
                    .WithProduct("^(Oculus Rift)|^(Oculus Quest)|^(Oculus Go)"));
            InputSystem.RegisterLayout<OculusTouchController>(
                matches: new InputDeviceMatcher()
                    .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
                    .WithProduct(@"(^(Oculus Touch Controller))|(^(Oculus Quest Controller))"));
            InputSystem.RegisterLayout<OculusRemote>(
                matches: new InputDeviceMatcher()
                    .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
                    .WithProduct(@"Oculus Remote"));
            InputSystem.RegisterLayout<OculusTrackingReference>(
                matches: new InputDeviceMatcher()
                    .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
                    .WithProduct(@"((Tracking Reference)|(^(Oculus Rift [a-zA-Z0-9]* \(Camera)))"));

            InputSystem.RegisterLayout<OculusHMDExtended>(
                name: "GearVR",
                matches: new InputDeviceMatcher()
                    .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
                    .WithProduct("Oculus HMD"));
            InputSystem.RegisterLayout<GearVRTrackedController>(
                matches: new InputDeviceMatcher()
                    .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
                    .WithProduct("^(Oculus Tracked Remote)"));
        }
    }
#endif

    public class OculusLoader : XRLoaderHelper
#if UNITY_EDITOR
    , IXRLoaderPreInit
#endif
    {
        private static List<XRDisplaySubsystemDescriptor> s_DisplaySubsystemDescriptors = new List<XRDisplaySubsystemDescriptor>();
        private static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors = new List<XRInputSubsystemDescriptor>();

        public XRDisplaySubsystem displaySubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRDisplaySubsystem>();
            }
        }

        public XRInputSubsystem inputSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRInputSubsystem>();
            }
        }

        public override bool Initialize()
        {
#if UNITY_INPUT_SYSTEM
            InputLayoutLoader.RegisterInputLayouts();
#endif

            OculusSettings settings = GetSettings();
            if (settings != null)
            {
                UserDefinedSettings userDefinedSettings;
                userDefinedSettings.sharedDepthBuffer = (ushort)(settings.SharedDepthBuffer ? 1 : 0);
                userDefinedSettings.dashSupport = (ushort)(settings.DashSupport ? 1 : 0);
                userDefinedSettings.stereoRenderingMode = (ushort) settings.GetStereoRenderingMode();
                userDefinedSettings.colorSpace = (ushort) ((QualitySettings.activeColorSpace == ColorSpace.Linear) ? 1 : 0);
                SetUserDefinedSettings(userDefinedSettings);
            }

            CreateSubsystem<XRDisplaySubsystemDescriptor, XRDisplaySubsystem>(s_DisplaySubsystemDescriptors, "oculus display");
            CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, "oculus input");
        
            if (displaySubsystem == null || inputSubsystem == null)
            {
                Debug.LogError("Unable to start Oculus XR Plugin.");
            }

            if (displaySubsystem == null)
            {
                Debug.LogError("Failed to load display subsystem.");
            }

            if (inputSubsystem == null)
            {
                Debug.LogError("Failed to load input subsystem.");
            }
 

            return displaySubsystem != null && inputSubsystem != null;
        }

        public override bool Start()
        {
            StartSubsystem<XRDisplaySubsystem>();
            StartSubsystem<XRInputSubsystem>();

            return true;
        }

        public override bool Stop()
        {
            StopSubsystem<XRDisplaySubsystem>();
            StopSubsystem<XRInputSubsystem>();

            return true;
        }

        public override bool Deinitialize()
        {
            DestroySubsystem<XRDisplaySubsystem>();
            DestroySubsystem<XRInputSubsystem>();

            return true;
        }

#if UNITY_EDITOR && XR_MGMT_GTE_320
        private void RemoveVulkanFromAndroidGraphicsAPIs()
        {
            // don't need to do anything if auto apis is selected
            if (PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.Android))
                return;

            GraphicsDeviceType[] oldApis = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
            List<GraphicsDeviceType> newApisList = new List<GraphicsDeviceType>();
            bool vulkanRemoved = false;

            // copy all entries except vulkan
            foreach (GraphicsDeviceType dev in oldApis)
            {
                if (dev == GraphicsDeviceType.Vulkan)
                {
                    vulkanRemoved = true;
                    continue;
                }
                
                newApisList.Add(dev);
            }

            // if we didn't remove Vulkan from the list, no need to do any further processing
            if (vulkanRemoved == false)
                return;

            if (newApisList.Count <= 0)
            {
                newApisList.Add(GraphicsDeviceType.OpenGLES3);
                Debug.LogWarning(
                    "Vulkan is currently experimental on Oculus Quest. It has been removed from your list of Android graphics APIs and replaced with OpenGLES3.\n" +
                    "If you would like to use experimental Quest Vulkan support, you can add it back into the list of graphics APIs in the Player settings.");
            }
            else
            {
                Debug.LogWarning(
                    "Vulkan is currently experimental on Oculus Quest. It has been removed from your list of Android graphics APIs.\n" +
                    "If you would like to use experimental Quest Vulkan support, you can add it back into the list of graphics APIs in the Player settings.");
            }

            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, newApisList.ToArray());
        }

        public override void WasAssignedToBuildTarget(BuildTargetGroup buildTargetGroup)
        {
            if (buildTargetGroup == BuildTargetGroup.Android)
            {
                RemoveVulkanFromAndroidGraphicsAPIs();
            }
        }
#endif

        [StructLayout(LayoutKind.Sequential)]
        struct UserDefinedSettings
        {
            public ushort sharedDepthBuffer;
            public ushort dashSupport;
            public ushort stereoRenderingMode;
            public ushort colorSpace;
        }

        [DllImport("OculusXRPlugin", CharSet=CharSet.Auto)]
        static extern void SetUserDefinedSettings(UserDefinedSettings settings);

        public OculusSettings GetSettings()
        {
            OculusSettings settings = null;
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject<OculusSettings>("Unity.XR.Oculus.Settings", out settings);
#else
            settings = OculusSettings.s_Settings;
#endif
            return settings;
        }

#if UNITY_EDITOR
        public string GetPreInitLibraryName(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup)
        {
            return "OculusXRPlugin";
        }
#endif
    }
}
