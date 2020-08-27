using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Management;
using UnityEngine.XR.Management.Tests;
using Object = UnityEngine.Object;

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX

namespace UnityEditor.XR.Management.Tests.BuildTests
{
#if UNITY_EDITOR_WIN
    [TestFixture(GraphicsDeviceType.Direct3D11, false, new [] { GraphicsDeviceType.Direct3D11})]
    [TestFixture(GraphicsDeviceType.Direct3D11, false, new [] { GraphicsDeviceType.Direct3D12, GraphicsDeviceType.Direct3D11})]
    [TestFixture(GraphicsDeviceType.Direct3D11, true, new [] { GraphicsDeviceType.Direct3D12, GraphicsDeviceType.Vulkan})]
    [TestFixture(GraphicsDeviceType.Direct3D11, false, new [] { GraphicsDeviceType.Null, GraphicsDeviceType.Vulkan})]
    [TestFixture(GraphicsDeviceType.Direct3D11, false, new [] { GraphicsDeviceType.Vulkan, GraphicsDeviceType.Null})]
#elif UNITY_EDITOR_OSX
    [TestFixture(GraphicsDeviceType.Metal, false, new [] { GraphicsDeviceType.Metal})]
    [TestFixture(GraphicsDeviceType.Metal, false, new [] { GraphicsDeviceType.Direct3D12, GraphicsDeviceType.Metal})]
    [TestFixture(GraphicsDeviceType.Metal, true, new [] { GraphicsDeviceType.OpenGLES3, GraphicsDeviceType.Vulkan})]
    [TestFixture(GraphicsDeviceType.Metal, false, new [] { GraphicsDeviceType.Null, GraphicsDeviceType.Vulkan})]
    [TestFixture(GraphicsDeviceType.Metal, false, new [] { GraphicsDeviceType.Vulkan, GraphicsDeviceType.Null})]
#endif
    class GraphicsAPICompatibilityTests
    {
        XRManagerSettings m_Manager;
        List<XRLoader> m_Loaders = new List<XRLoader>();

        private GraphicsDeviceType m_PlayerSettingsDeviceType;
        private GraphicsDeviceType[]  m_LoadersSupporteDeviceTypes;
        bool m_BuildFails;

        public GraphicsAPICompatibilityTests(GraphicsDeviceType playerSettingsDeviceType, bool fails, GraphicsDeviceType[] loaders)
        {
            m_BuildFails = fails;
            m_PlayerSettingsDeviceType = playerSettingsDeviceType;
            m_LoadersSupporteDeviceTypes = loaders;
        }

        [SetUp]
        public void SetupPlayerSettings()
        {
#if UNITY_EDITOR_WIN
            PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneWindows64, new[] { m_PlayerSettingsDeviceType });
#elif UNITY_EDITOR_OSX
            PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneOSX, new[] { m_PlayerSettingsDeviceType });
#endif
            m_Manager = ScriptableObject.CreateInstance<XRManagerSettings>();
            m_Manager.automaticLoading = false;

            m_Loaders = new List<XRLoader>();

            for (int i = 0; i < m_LoadersSupporteDeviceTypes.Length; i++)
            {
                DummyLoader dl = ScriptableObject.CreateInstance(typeof(DummyLoader)) as DummyLoader;
                dl.id = i;
                dl.supportedDeviceType = m_LoadersSupporteDeviceTypes[i];
                m_Loaders.Add(dl);
                m_Manager.loaders.Add(dl);
            }
        }

        [TearDown]
        public void TeadDown()
        {
            Object.DestroyImmediate(m_Manager);
            m_Manager = null;

        }

        [Test]
        public void CheckGraphicsAPICompatibilityOnBuild()
        {
            try
            {
                XRGeneralBuildProcessor.VerifyGraphicsAPICompatibility(m_Manager, m_PlayerSettingsDeviceType);
            }
            catch (BuildFailedException)
            {
                Assert.True(m_BuildFails);
                return;
            }

            Assert.False(m_BuildFails);
        }
    }
}
#endif //UNITY_EDITOR_WIN || UNITY_EDITOR_OSX