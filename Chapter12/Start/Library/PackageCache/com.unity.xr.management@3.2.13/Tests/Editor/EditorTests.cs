using NUnit.Framework;

using System;
using System.IO;

using UnityEngine;
using UnityEngine.XR.Management;

using Unity.XR.TestTooling;

namespace UnityEditor.XR.Management.Tests
{

    class XRGeneralSettingsTests : ManagementTestSetup
    {
        protected override bool TestManagerUpgradePath => true;
        BuildTargetGroup previousBuildTargetSelection { get; set;  }

        [SetUp]
        public override void SetupTest()
        {
            
            base.SetupTest();

            previousBuildTargetSelection = EditorUserBuildSettings.selectedBuildTargetGroup;
            EditorUserBuildSettings.selectedBuildTargetGroup = BuildTargetGroup.Standalone;
        }

        [TearDown]
        public override void TearDownTest()
        {
            EditorUserBuildSettings.selectedBuildTargetGroup = previousBuildTargetSelection;
            base.TearDownTest();
        }


        [Test]
        public void UpdateGeneralSettings_ToPerBuildTargetSettings()
        {
            bool success = XRGeneralSettingsUpgrade.UpgradeSettingsToPerBuildTarget(testPathToSettings);
            Assert.IsTrue(success);

            XRGeneralSettingsPerBuildTarget pbtgs = null;

            pbtgs = AssetDatabase.LoadAssetAtPath(testPathToSettings, typeof(XRGeneralSettingsPerBuildTarget)) as XRGeneralSettingsPerBuildTarget;
            Assert.IsNotNull(pbtgs);

            var settings = pbtgs.SettingsForBuildTarget(EditorUserBuildSettings.selectedBuildTargetGroup);
            Assert.IsNotNull(settings);
            Assert.IsNotNull(settings.Manager);
            Assert.AreEqual(testManager, settings.Manager);
        }
    }
}
