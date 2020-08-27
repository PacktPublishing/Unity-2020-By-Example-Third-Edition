using System.Collections.Generic;

using UnityEditorInternal;
using UnityEngine;

using UnityEditor.XR.Management.Metadata;

namespace UnityEditor.XR.Management
{

    internal interface IXRLoaderOrderManager
    {
        List<XRLoaderInfo> AssignedLoaders { get; }
        List<XRLoaderInfo> UnassignedLoaders { get; }

        void AssignLoader(XRLoaderInfo assignedInfo);
        void UnassignLoader(XRLoaderInfo unassignedInfo);
        void Update();
    }

    internal class XRLoaderOrderUI
    {
        struct LoaderInformation
        {
            public string packageName;
            public string packageId;
            public string loaderName;
            public string loaderType;
            public bool toggled;
            public bool stateChanged;
        }

        const string k_AtNoLoaderInstance = "There are no XR plugins applicable to this platform.";
        private List<LoaderInformation> m_LoaderMetadata = null;


        ReorderableList m_OrderedList = null;

        public BuildTargetGroup CurrentBuildTargetGroup { get; set; }

        internal XRLoaderOrderUI()
        {
        }

        void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var li = m_LoaderMetadata[index];

            li.toggled = XRPackageMetadataStore.IsLoaderAssigned(li.loaderType, CurrentBuildTargetGroup);
            bool preToggledState = li.toggled;
            li.toggled = EditorGUI.ToggleLeft(rect, li.loaderName, preToggledState);
            if (li.toggled != preToggledState)
            {
                li.stateChanged = true;
                m_LoaderMetadata[index] = li;
            }
        }

        float GetElementHeight(int index)
        {
            return m_OrderedList.elementHeight;
        }

        internal bool OnGUI(BuildTargetGroup buildTargetGroup)
        {
            var settings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(buildTargetGroup);

            if (buildTargetGroup != CurrentBuildTargetGroup || m_LoaderMetadata == null)
            {
                CurrentBuildTargetGroup = buildTargetGroup;

                if (m_LoaderMetadata == null)
                    m_LoaderMetadata = new List<LoaderInformation>();
                else
                    m_LoaderMetadata.Clear();
                
                foreach (var pmd in XRPackageMetadataStore.GetLoadersForBuildTarget(buildTargetGroup))
                {
                    m_LoaderMetadata.Add(new LoaderInformation() {
                        packageName = pmd.packageName,
                        packageId = pmd.packageId,
                        loaderName = pmd.loaderName,
                        loaderType = pmd.loaderType,
                        toggled = XRPackageMetadataStore.IsLoaderAssigned(pmd.loaderType, buildTargetGroup)
                    });
                }

                if (settings != null)
                {
                    LoaderInformation li;
                    for (int i = 0; i < m_LoaderMetadata.Count; i++)
                    {
                        li = m_LoaderMetadata[i];                        
                        if (XRPackageMetadataStore.IsLoaderAssigned(settings.AssignedSettings, li.loaderType))
                        {
                            li.toggled = true;
                            m_LoaderMetadata[i] = li;
                            break;
                        }
                    }
                }

                m_OrderedList = new ReorderableList(m_LoaderMetadata, typeof(LoaderInformation), false, true, false, false);
                m_OrderedList.drawHeaderCallback = (rect) => GUI.Label(rect, EditorGUIUtility.TrTextContent("Plug-in Providers"), EditorStyles.label);
                m_OrderedList.drawElementCallback = (rect, index, isActive, isFocused) => DrawElementCallback(rect, index, isActive, isFocused);
                m_OrderedList.drawFooterCallback = (rect) =>
                {
                    var status = XRPackageMetadataStore.GetCurrentStatusDisplayText();
                    GUI.Label(rect, EditorGUIUtility.TrTextContent(status), EditorStyles.label);
                };
                m_OrderedList.elementHeightCallback = (index) => GetElementHeight(index);
            }
            
            if (m_LoaderMetadata == null || m_LoaderMetadata.Count == 0)
            {
                EditorGUILayout.HelpBox(k_AtNoLoaderInstance, MessageType.Info);
            }
            else
            {
                m_OrderedList.DoLayoutList();
                if (settings != null)
                {
                    LoaderInformation li;
                    for (int i = 0; i < m_LoaderMetadata.Count; i++)
                    {
                        li = m_LoaderMetadata[i];    
                        if (li.stateChanged)
                        {
                            if (li.toggled)
                            {
                                XRPackageMetadataStore.InstallPackageAndAssignLoaderForBuildTarget(li.packageId, li.loaderType, buildTargetGroup);
                            }
                            else
                            {
                                XRPackageMetadataStore.RemoveLoader(settings.AssignedSettings, li.loaderType, buildTargetGroup);
                            }
                            li.stateChanged = false;
                            m_LoaderMetadata[i] = li;
                        }
                    }
                }
            }

            return false;
        }
    }
}
