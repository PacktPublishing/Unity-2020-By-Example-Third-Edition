#if UNITY_2020_2_OR_NEWER
using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

using UnityEditor;

namespace MockHMD.Editor.MultiCamera
{
    public class AddCameraWindow : EditorWindow
    {
        private List<MockCamera> camerasToRemove = new List<MockCamera>();
        private AdditionalMockCameras additionalMockCameras = new AdditionalMockCameras();
        private Vector2 camerasScrollPos;

        private static NativeApi.MockCameraProperties cameraProperties = new NativeApi.MockCameraProperties{position = new NativeApi.float3(), rotation = new NativeApi.float4()};
        private static List<XRDisplaySubsystem> s_displaySubsystems = new List<XRDisplaySubsystem>();

        private int m_CameraIds = 0;
        private bool isPlaying = false;

       XRDisplaySubsystem GetXRDisplaySubsystem()
        {
            XRDisplaySubsystem ret = null;
            SubsystemManager.GetInstances<XRDisplaySubsystem>(s_displaySubsystems);
            if (s_displaySubsystems.Count > 0)
            {
                ret = s_displaySubsystems[0];
            }
            return ret;
        }

        [MenuItem ("Window/XR/Mock HMD/Additional Cameras")]
        public static void ShowWindow () 
        {
            EditorWindow w = EditorWindow.GetWindow(typeof(AddCameraWindow));
            w.titleContent = new GUIContent("Mock HMD Add Camera");
        }

        void OnEnable()
        {
            if (EditorPrefs.HasKey("MOCK Cameras"))
            {
                string data = EditorPrefs.GetString("MOCK Cameras");
                JsonUtility.FromJsonOverwrite(data, additionalMockCameras);
                foreach (var mockCam in additionalMockCameras.extraCameras)
                {
                    mockCam.id = -1;
                }
            }

            EditorApplication.playModeStateChanged += SetCamerasOnPlayStateChanged;
        }

        void OnDisable()
        {
            EditorApplication.playModeStateChanged -= SetCamerasOnPlayStateChanged;

            string data = JsonUtility.ToJson(additionalMockCameras);
            EditorPrefs.SetString("MOCK Cameras", data);
        }

        void SetCamerasOnPlayStateChanged(PlayModeStateChange state)
        {
            switch(state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    foreach (var mockCam in additionalMockCameras.extraCameras)
                    {
                        if (mockCam.isActive)
                        {
                            if (mockCam.id < 0)
                            {
                                mockCam.id = m_CameraIds;
                                m_CameraIds++;
                            }

                            if (!NativeApi.HasCameraWithId(mockCam.id))
                                AddCamera(mockCam.id);
                        }
                    }
                    isPlaying = true;
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    isPlaying = false;
                    foreach (var mockCam in additionalMockCameras.extraCameras)
                    {
                        if (mockCam.isActive)
                        {
                            if (NativeApi.HasCameraWithId(mockCam.id))
                                RemoveCamera(mockCam.id);
                            mockCam.id = -1;
                        }
                    }
                    break;
            }
        }

        void UpdateAllCameras()
        {
            if (isPlaying)
            {
                foreach (var mockCam in additionalMockCameras.extraCameras)
                {
                    if (mockCam.isActive)
                    {
                        if (mockCam.id < 0)
                        {
                            mockCam.id = m_CameraIds;
                            m_CameraIds++;
                        }

                        if (!NativeApi.HasCameraWithId(mockCam.id))
                            AddCamera(mockCam.id);
                        UpdateCamera(mockCam.id, mockCam);
                    }
                    else
                    {
                        if (NativeApi.HasCameraWithId(mockCam.id))
                            RemoveCamera(mockCam.id);
                    }
                }
            }
        }

        void OnGUI()
        {
            var displaySubsystem = GetXRDisplaySubsystem();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space();
            if (GUILayout.Button("Add Camera"))
            {
                var newCam = new MockCamera();
                newCam.id = -1;
                additionalMockCameras.extraCameras.Add(newCam);
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            EditorGUILayout.Space();

            int camIndex = 0;
            camerasScrollPos = EditorGUILayout.BeginScrollView(camerasScrollPos, GUILayout.Width(EditorGUIUtility.currentViewWidth));
            foreach (var mockCam in additionalMockCameras.extraCameras)
            {
                EditorGUILayout.LabelField($"Mock Camera {camIndex}", EditorStyles.largeLabel);
                EditorGUILayout.Space();

                camIndex++;

                mockCam.position    = EditorGUILayout.Vector3Field("Position", mockCam.position);
                mockCam.rotation.eulerAngles    = EditorGUILayout.Vector3Field("Rotation", mockCam.rotation.eulerAngles);
                mockCam.fov         = EditorGUILayout.FloatField("Field of View", mockCam.fov);
                mockCam.near        = EditorGUILayout.FloatField("Near", mockCam.near);
                mockCam.far         = EditorGUILayout.FloatField("Far", mockCam.far);
                mockCam.textureWidth  = EditorGUILayout.IntField("Texture Width", mockCam.textureWidth);
                mockCam.textureHeight = EditorGUILayout.IntField("Texture Height", mockCam.textureHeight);

                mockCam.stereoRenderingMode = (MockRenderingMode)EditorGUILayout.EnumPopup("Rendering Mode", mockCam.stereoRenderingMode);
                if (mockCam.stereoRenderingMode != MockRenderingMode.None)
                {
                    mockCam.eyeSeparation = EditorGUILayout.FloatField("Eye Separation", mockCam.eyeSeparation);
                    mockCam.eyeSeparation = Math.Max(Math.Min(5.0f, mockCam.eyeSeparation), 0.01f);
                }

                mockCam.enableLeftOcclusion = EditorGUILayout.Toggle("Left Occlusion: ", mockCam.enableLeftOcclusion);
                mockCam.leftAbberation = EditorGUILayout.Vector2Field("Left Abberation: ", mockCam.leftAbberation);

                EditorGUILayout.Space();
                mockCam.enableRightOcclusion = EditorGUILayout.Toggle("Right Occlusion: ", mockCam.enableRightOcclusion);
                mockCam.rightAbberation = EditorGUILayout.Vector2Field("Right Abberation: ", mockCam.rightAbberation);

                bool preIsActive = mockCam.isActive;
                mockCam.isActive = EditorApplication.isPlayingOrWillChangePlaymode ? EditorGUILayout.Toggle("Active", mockCam.isActive) : false;

                if (GUILayout.Button("Remove Camera"))
                {
                    camerasToRemove.Add(mockCam);
                    mockCam.isActive = false;
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
            EditorGUILayout.EndScrollView();


            EditorGUILayout.EndVertical();


            UpdateAllCameras();

            foreach (var mockCam in camerasToRemove)
            {
                additionalMockCameras.extraCameras.Remove(mockCam);
            }
        }

        private void AddCamera(int id)
        {
            if (NativeApi.HasCameraWithId(id))
            {
                Debug.LogWarning($"Attmept to add a camera with id {id} when one already exists!");
                return;
            }

            if (!NativeApi.AddCameraWithId(id))
            {
                Debug.LogError($"Attempt to add camera with id {id} failed!");
            }
        }

        private void UpdateCamera(int id, MockCamera mockCam)
        {
            if (NativeApi.HasCameraWithId(id))
            {
                cameraProperties.position.x = mockCam.position.x;
                cameraProperties.position.y = mockCam.position.y;
                cameraProperties.position.z = mockCam.position.z;

                cameraProperties.rotation.x = mockCam.rotation.x;
                cameraProperties.rotation.y = mockCam.rotation.y;
                cameraProperties.rotation.z = mockCam.rotation.z;
                cameraProperties.rotation.w = mockCam.rotation.w;

                cameraProperties.fov = mockCam.fov;
                cameraProperties.near = mockCam.near;
                cameraProperties.far = mockCam.far;
                cameraProperties.textureWidth = mockCam.textureWidth;
                cameraProperties.textureHeight = mockCam.textureHeight;
                cameraProperties.renderingMode = (int)mockCam.stereoRenderingMode;
                cameraProperties.eyeSeparation = mockCam.eyeSeparation;
                cameraProperties.leftAbberation.x = mockCam.leftAbberation.x;
                cameraProperties.leftAbberation.y = mockCam.leftAbberation.y;
                cameraProperties.leftAbberation.z = 0;

                cameraProperties.rightAbberation.x = mockCam.rightAbberation.x;
                cameraProperties.rightAbberation.y = mockCam.rightAbberation.y;
                cameraProperties.rightAbberation.z = 0;

                cameraProperties.enableLeftOcclusion = mockCam.enableLeftOcclusion ? 1 : 0;
                cameraProperties.enableRightOcclusion = mockCam.enableRightOcclusion ? 1 : 0;

                NativeApi.UpdateCameraWithId(id, cameraProperties);
            }

        }

        private void RemoveCamera(int id)
        {
            if (!NativeApi.RemoveCameraWithId(id))
            {
                Debug.LogWarning($"Could not remove camera with id {id}");
            }
        }
    }
}
#endif
