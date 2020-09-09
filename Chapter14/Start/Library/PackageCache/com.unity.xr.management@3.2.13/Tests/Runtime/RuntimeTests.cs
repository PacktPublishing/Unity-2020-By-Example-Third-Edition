using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;

using UnityEditor;
using UnityEngine.Rendering;

namespace UnityEngine.XR.Management.Tests
{
    [TestFixture(0, -1)] // No loaders, should never have any results
    [TestFixture(1, -1)] // 1 loader, fails so no active loaders
    [TestFixture(1, 0)] // All others, make sure the active loader is expected loader.
    [TestFixture(2, 0)]
    [TestFixture(2, 1)]
    [TestFixture(3, 2)]
    class ManualLifetimeTests
    {
        XRManagerSettings m_Manager;
        List<XRLoader> m_Loaders = new List<XRLoader>();
        int m_LoaderCount;
        int m_LoaderIndexToWin;

        public ManualLifetimeTests(int loaderCount, int loaderIndexToWin)
        {
            m_LoaderCount = loaderCount;
            m_LoaderIndexToWin = loaderIndexToWin;
        }

        [SetUp]
        public void SetupXRManagerTest()
        {
            m_Manager = ScriptableObject.CreateInstance<XRManagerSettings>();
            m_Manager.automaticLoading = false;

            m_Loaders = new List<XRLoader>();

            for (int i = 0; i < m_LoaderCount; i++)
            {
                DummyLoader dl = ScriptableObject.CreateInstance(typeof(DummyLoader)) as DummyLoader;
                dl.id = i;
                dl.shouldFail = (i != m_LoaderIndexToWin);
                m_Loaders.Add(dl);
                m_Manager.loaders.Add(dl);
            }
        }

        [TearDown]
        public void TeardownXRManagerTest()
        {
            Object.Destroy(m_Manager);
            m_Manager = null;
        }

        [UnityTest]
        public IEnumerator CheckActivatedLoader()
        {
            Assert.IsNotNull(m_Manager);

            yield return m_Manager.InitializeLoader();

            if (m_LoaderIndexToWin < 0 || m_LoaderIndexToWin >= m_Loaders.Count)
            {
                Assert.IsNull(m_Manager.activeLoader);
            }
            else
            {
                Assert.IsNotNull(m_Manager.activeLoader);
                Assert.AreEqual(m_Loaders[m_LoaderIndexToWin], m_Manager.activeLoader);
            }

            m_Manager.DeinitializeLoader();

            Assert.IsNull(m_Manager.activeLoader);

            m_Manager.loaders.Clear();
        }
    }

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX

#if UNITY_EDITOR_WIN
    [TestFixture(GraphicsDeviceType.Direct3D11, 0, new [] { GraphicsDeviceType.Direct3D11})]
    [TestFixture(GraphicsDeviceType.Direct3D11, 1, new [] { GraphicsDeviceType.Direct3D12, GraphicsDeviceType.Direct3D11})]
    [TestFixture(GraphicsDeviceType.Direct3D11, -1, new [] { GraphicsDeviceType.Direct3D12, GraphicsDeviceType.Vulkan})]
    [TestFixture(GraphicsDeviceType.Direct3D11, 0, new [] { GraphicsDeviceType.Null, GraphicsDeviceType.Vulkan})]
    [TestFixture(GraphicsDeviceType.Direct3D11, 1, new [] { GraphicsDeviceType.Vulkan, GraphicsDeviceType.Null})]
#elif UNITY_EDITOR_OSX
    [TestFixture(GraphicsDeviceType.Metal, 0, new [] { GraphicsDeviceType.Metal})]
    [TestFixture(GraphicsDeviceType.Metal, 1, new [] { GraphicsDeviceType.Direct3D12, GraphicsDeviceType.Metal})]
    [TestFixture(GraphicsDeviceType.Metal, -1, new [] { GraphicsDeviceType.OpenGLES3, GraphicsDeviceType.Vulkan})]
    [TestFixture(GraphicsDeviceType.Metal, 0, new [] { GraphicsDeviceType.Null, GraphicsDeviceType.Vulkan})]
    [TestFixture(GraphicsDeviceType.Metal, 1, new [] { GraphicsDeviceType.Vulkan, GraphicsDeviceType.Null})]
#endif
    class GraphicsAPICompatibilityTests
    {
        XRManagerSettings m_Manager;
        List<XRLoader> m_Loaders = new List<XRLoader>();

        private GraphicsDeviceType m_PlayerSettingsDeviceType;
        private GraphicsDeviceType[]  m_LoadersSupporteDeviceTypes;
        int m_LoaderIndexToWin;
        public GraphicsAPICompatibilityTests(GraphicsDeviceType playerSettingsDeviceType,  int indexToWin, GraphicsDeviceType[] loaders)
        {
            m_LoaderIndexToWin = indexToWin;
            m_PlayerSettingsDeviceType = playerSettingsDeviceType;
            m_LoadersSupporteDeviceTypes = loaders;
        }

        [SetUp]
        public void SetupPlayerSettings()
        {
            GraphicsDeviceType[] deviceTypes = PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneOSX);
            var oldGfxType = m_PlayerSettingsDeviceType;

            // If the type we want to check isn't the supported graphics type, then substitute it out
            // so we can still pass the tests. Semantics are the same regardless of actual devices.
            if (SystemInfo.graphicsDeviceType != m_PlayerSettingsDeviceType)
            {
                m_PlayerSettingsDeviceType = SystemInfo.graphicsDeviceType;

                for (int i = 0; i < m_LoadersSupporteDeviceTypes.Length; i++)
                {
                    if (oldGfxType == m_LoadersSupporteDeviceTypes[i])
                    {
                        m_LoadersSupporteDeviceTypes[i] = m_PlayerSettingsDeviceType;
                    }
                }
            }

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
                dl.shouldFail = (i != m_LoaderIndexToWin);
                m_Loaders.Add(dl);
                m_Manager.loaders.Add(dl);
            }
        }

        [TearDown]
        public void TeadDown()
        {
            Object.Destroy(m_Manager);
            m_Manager = null;
        }

        [Test]
        public void CheckGraphicsAPICompatibilitySync()
        {
            m_Manager.InitializeLoaderSync();

            if (m_LoaderIndexToWin < 0 || m_LoaderIndexToWin >= m_Loaders.Count)
            {
                Assert.IsNull(m_Manager.activeLoader);
            }
            else
            {
                Assert.IsNotNull(m_Manager.activeLoader);
                Assert.AreEqual(m_Loaders[m_LoaderIndexToWin], m_Manager.activeLoader);
                m_Manager.DeinitializeLoader();
            }

            m_Manager.loaders.Clear();
        }

        [UnityTest]
        public IEnumerator CheckGraphicsAPICompatibility()
        {
            yield return m_Manager.InitializeLoader();

            if (m_LoaderIndexToWin < 0 || m_LoaderIndexToWin >= m_Loaders.Count)
            {
                Assert.IsNull(m_Manager.activeLoader);
            }
            else
            {
                Assert.IsNotNull(m_Manager.activeLoader);
                Assert.AreEqual(m_Loaders[m_LoaderIndexToWin], m_Manager.activeLoader);
                m_Manager.DeinitializeLoader();
            }

            m_Manager.loaders.Clear();
        }
    }
#endif // UNITY_EDITOR_WIN || UNITY_EDITOR_OSX

}
