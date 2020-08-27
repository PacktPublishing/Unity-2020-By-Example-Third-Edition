using UnityEngine.XR;

namespace Unity.XR.Oculus
{
    /// <summary>
    /// Input Usages, consumed by the UnityEngine.XR.InputDevice class in order to retrieve inputs.
    /// These usages are all Oculus specific.
    /// </summary>
    public static class OculusUsages
    {
        /// <summary>
        /// Represents the state of the volume up physical button on the headset.  This is also processed internally to adjust the volume regardless of developer handling.
        /// </summary>
        public static InputFeatureUsage<bool> volumeUp = new InputFeatureUsage<bool>("VolumeUp");
        /// <summary>
        /// Represents the state of the volume down physical button on the headset.  This is also processed internally to adjust the volume regardless of developer handling.
        /// </summary>
        public static InputFeatureUsage<bool> volumeDown = new InputFeatureUsage<bool>("VolumeDown");
        /// <summary>
        /// Represents the capacitive touch thumbrest on Oculus Rift controllers.
        /// </summary>
        public static InputFeatureUsage<bool> thumbrest = new InputFeatureUsage<bool>("Thumbrest");
        /// <summary>
        /// Represents the capacitive touch sensor state on the trigger of the Oculus Rift Controller.
        /// </summary>
        public static InputFeatureUsage<bool> indexTouch = new InputFeatureUsage<bool>("IndexTouch");
        /// <summary>
        /// Represents the capacitive touch sensor state on the grip of the Oculus Rift Controller.
        /// </summary>
        public static InputFeatureUsage<bool> thumbTouch = new InputFeatureUsage<bool>("ThumbTouch");
    }
}