using NUnit.Framework;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems.Tests
{
    public class XRAnchorSubsystemImpl : XRAnchorSubsystem
    {
        protected override Provider CreateProvider() => new TestProvider();

        class TestProvider : Provider
        {
            public override TrackableChanges<XRAnchor> GetChanges(XRAnchor defaultAnchor, Allocator allocator) => default;
        }
    }

    [TestFixture]
    public class XRAnchorSubsystemTestFixture
    {
        [Test]
        public void RunningStateTests()
        {
            XRAnchorSubsystem subsystem = new XRAnchorSubsystemImpl();

            // Initial state is not running
            Assert.That(subsystem.running == false);

            // After start subsystem is running
            subsystem.Start();
            Assert.That(subsystem.running == true);

            // After start subsystem is running
            subsystem.Stop();
            Assert.That(subsystem.running == false);
        }
    }
}