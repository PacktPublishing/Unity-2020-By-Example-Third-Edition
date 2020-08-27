using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.XR.Oculus
{
    public static partial class Utils
    {
        /// <summary>
        /// Set the degree of foveation.  See [Oculus Documention](https://developer.oculus.com/documentation/quest/latest/concepts/mobile-ffr/).
        /// </summary>
        /// <param name="level">
        ///  level can be 0, 1, 2, 3, or 4:
        /// 
        /// * 0 disables multi-resolution
        /// * 1 low FFR setting
        /// * 2 medium FFR setting
        /// * 3 high FFR setting
        /// * 4 high top FFR setting
        /// </param>
        public static void SetFoveationLevel(int level)
        {
            IntPtr ovrJava = GetOvrJava();
            if (ovrJava == IntPtr.Zero)
            {
                Debug.LogError("Can't set foveation level");
                return;
            }
            vrapi_SetPropertyInt(ovrJava, OvrProperty.FoveationLevel, level);
        }

        private enum OvrProperty
        {
            FoveationLevel = 15,
        }

        [DllImport("vrapi", EntryPoint = "vrapi_SetPropertyInt")]
        private static extern void vrapi_SetPropertyInt(IntPtr java, OvrProperty prop, int val);

        [DllImport("OculusXRPlugin")]
        private static extern IntPtr GetOvrJava();
    }
}
