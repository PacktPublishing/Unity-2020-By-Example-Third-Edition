using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.XR.Oculus
{
    public static partial class Utils
    {
        /// <summary>
        /// Set the color scale and color offset of the eye texture layers
        /// </summary>
        /// <param name="colorScale">Scales the eye layer texture color by this Vector4.</param>
        /// <param name="colorOffset">Offsets the eye layer texture color by this Vector4</param>
        public static void SetColorScaleAndOffset(Vector4 colorScale, Vector4 colorOffset)
        {
            SetColorScale(colorScale.x, colorScale.y, colorScale.z, colorScale.w);
            SetColorOffset(colorOffset.x, colorOffset.y, colorOffset.z, colorOffset.w);
        }

       [DllImport("OculusXRPlugin", CharSet=CharSet.Auto)]
        static extern void SetColorScale(float x, float y, float z, float w);

        
        [DllImport("OculusXRPlugin", CharSet=CharSet.Auto)]
        static extern void SetColorOffset(float x, float y, float z, float w);
    }

}