#if UNITY_2020_2_OR_NEWER
using System;
using System.Collections.Generic;

using UnityEngine;

namespace MockHMD.Editor.MultiCamera
{
    public enum MockRenderingMode
    {
        None,
        MultiPass,
        SinglePassInstance,
    }

    [System.Serializable]
    public class MockCamera
    {
        [SerializeField]
        public int id;
        [SerializeField]
        public Vector3 position;
        [SerializeField]
        public Quaternion rotation;
        [SerializeField]
        public float fov = 60.0f;
        [SerializeField]
        public float near = 0.3f;
        [SerializeField]
        public float far = 1000.0f;
        [SerializeField]
        public int textureWidth = 100;
        [SerializeField]
        public int textureHeight = 100;

        [SerializeField]
        public MockRenderingMode stereoRenderingMode = MockRenderingMode.None;
        [SerializeField]
        public float eyeSeparation = 0.2f;
        [SerializeField]
        public Vector2 leftAbberation;
        [SerializeField]
        public Vector2 rightAbberation;
        [SerializeField]
        public bool enableLeftOcclusion;
        [SerializeField]
        public bool enableRightOcclusion;
        
        [SerializeField]
        public bool isActive;

    }


    [System.Serializable]
    public class AdditionalMockCameras
    {
        [SerializeField]
        public List<MockCamera> extraCameras = new List<MockCamera>();
    }
}
#endif
