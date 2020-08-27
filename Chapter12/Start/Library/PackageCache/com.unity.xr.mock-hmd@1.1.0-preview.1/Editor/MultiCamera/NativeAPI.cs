#if UNITY_2020_2_OR_NEWER
using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace MockHMD.Editor.MultiCamera
{
    public static class NativeApi
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct float3
        {
            public float x;
            public float y;
            public float z;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct float4
        {
            public float x;
            public float y;
            public float z;
            public float w;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MockCameraProperties
        {
            public float3 position;
            public float4 rotation;
            public float fov;
            public float near;
            public float far;
            public int textureWidth;
            public int textureHeight;
            public int renderingMode;
            public float eyeSeparation;
            public float3 leftAbberation;
            public float3 rightAbberation;
            public int enableLeftOcclusion;
            public int enableRightOcclusion;
        }

        [DllImport("Packages/com.unity.xr.mock-hmd/Runtime/Windows/x64/UnityMockHMD.dll", CharSet = CharSet.Auto)]
        public static extern bool HasCameraWithId(int id);

        [DllImport("Packages/com.unity.xr.mock-hmd/Runtime/Windows/x64/UnityMockHMD.dll", CharSet = CharSet.Auto)]
        public static extern bool AddCameraWithId(int id);

        [DllImport("Packages/com.unity.xr.mock-hmd/Runtime/Windows/x64/UnityMockHMD.dll", CharSet = CharSet.Auto)]
        public static extern bool UpdateCameraWithId(int id, MockCameraProperties properties);

        [DllImport("Packages/com.unity.xr.mock-hmd/Runtime/Windows/x64/UnityMockHMD.dll", CharSet = CharSet.Auto)]
        public static extern bool RemoveCameraWithId(int id);

    }
}
#endif
