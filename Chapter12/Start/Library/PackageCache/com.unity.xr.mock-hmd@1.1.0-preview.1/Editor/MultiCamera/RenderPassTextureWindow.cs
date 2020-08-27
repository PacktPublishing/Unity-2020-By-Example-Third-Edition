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
    public class RenderPassTextureWindow : EditorWindow
    {

        Vector2 renderTextureScrollPos;

        int textureImageHeight = 200;
        int textureImageWidth = 200;
        int textureImageSpace = 20;
        int textureImageLeftPadding = 25;

        private static List<XRDisplaySubsystem> s_displaySubsystems = new List<XRDisplaySubsystem>();
        XRDisplaySubsystem.XRRenderPass s_renderPass = new XRDisplaySubsystem.XRRenderPass();
        XRDisplaySubsystem.XRRenderParameter s_renderParameter= new XRDisplaySubsystem.XRRenderParameter();


        [MenuItem ("Window/XR/Mock HMD/Render Texture Viewer")]
        public static void ShowWindow () 
        {
            EditorWindow w = EditorWindow.GetWindow(typeof(RenderPassTextureWindow));
            w.titleContent = new GUIContent("Render Texture Viewer");
        }

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

        private void Update()
        {
            if (EditorApplication.isPlaying)
            {
                Repaint();
            }
        }

        void DisplayRenderTextures(XRDisplaySubsystem displaySubsystem)
        {
            RenderTexture rt = displaySubsystem.GetRenderTextureForRenderPass(s_renderPass.renderPassIndex);
            if (rt == null)
                return;

            int rpc = s_renderPass.GetRenderParameterCount();
            RenderTexture target = new RenderTexture(rt.width, rt.height, rt.depth);

            Graphics.CopyTexture(rt, 0, target, 0);
            EditorGUILayout.BeginHorizontal();

            Rect r = EditorGUILayout.GetControlRect(false, textureImageHeight,
                GUILayout.MaxWidth(textureImageWidth + textureImageLeftPadding),
                GUILayout.Width(textureImageWidth),
                GUILayout.MaxHeight(textureImageHeight + textureImageSpace));

            EditorGUI.DrawPreviewTexture(r, target);

            if (rpc > 1)
            {
                Graphics.CopyTexture(rt, 1, target, 0);

                r = EditorGUILayout.GetControlRect(false, textureImageHeight,
                    GUILayout.MaxWidth(textureImageWidth + textureImageLeftPadding),
                    GUILayout.Width(textureImageWidth),
                    GUILayout.MaxHeight(textureImageHeight + textureImageSpace));


                EditorGUI.DrawPreviewTexture(r, target);

            }
            target.DiscardContents();
            target.Release();

            EditorGUILayout.EndHorizontal();

        }

        void DisplayRenderPassData()
        {
            EditorGUILayout.BeginHorizontal();
            using (new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField($"Render Pass : {s_renderPass.renderPassIndex}");
                EditorGUILayout.LabelField($"Should Fill Depth : {s_renderPass.shouldFillOutDepth}");
                EditorGUILayout.LabelField($"Cull Index: {s_renderPass.cullingPassIndex}");
            }

            int rpc = s_renderPass.GetRenderParameterCount();
            for (int rpmi = 0; rpmi < rpc; rpmi++)
            {
                try
                {
                    s_renderPass.GetRenderParameter(UnityEngine.Camera.main, rpmi, out s_renderParameter);
                    using (new EditorGUILayout.VerticalScope())
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField($"Render Parameter: {rpmi}");
                        var planes = s_renderParameter.projection.decomposeProjection;
                        using (new EditorGUI.DisabledScope(true))
                        {
                            EditorGUILayout.RectField("Projection Planes", new Rect(planes.left, planes.top, planes.right, planes.bottom));
                        }
                    }
                }
                catch (Exception)
                {

                }

            }
            EditorGUILayout.EndHorizontal();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            
            if (EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                var displaySubsystem = GetXRDisplaySubsystem();
                if (displaySubsystem == null)
                    return;

#if !UNITY_20202_2_OR_NEWER
                if (displaySubsystem.disableLegacyRenderer)
                    return;
#endif

                int renderPassCount = displaySubsystem.GetRenderPassCount();

                EditorGUILayout.LabelField("Render Pass Textures:");
                renderTextureScrollPos = EditorGUILayout.BeginScrollView(renderTextureScrollPos, GUILayout.Width(EditorGUIUtility.currentViewWidth));

                for (int rpi = 0; rpi < renderPassCount; rpi++)
                {
                    displaySubsystem.GetRenderPass(rpi, out s_renderPass);

                    EditorGUILayout.Space();
                    DisplayRenderPassData();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                    DisplayRenderTextures(displaySubsystem);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }

                EditorGUILayout.EndScrollView();

            }
            else
            {
                EditorGUILayout.LabelField("Must be running in Play mode to see render textures.");
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}
#endif
