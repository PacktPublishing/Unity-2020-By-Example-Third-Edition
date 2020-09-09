using System;
using UnityEngine.XR.Management;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// Settings to control the ARCoreLoader behavior.
    /// </summary>
    [System.Serializable]
    public class ARCoreLoaderSettings : ScriptableObject
    {
        [Obsolete("ARCoreLoader no longer makes use of this setting. Subsystems are stopped and started based only on XR Managment's general initialization setting.", false)]
        public bool startAndStopSubsystems
        {
            get { return false; }
            set { }
        }

        [Obsolete("This method has been deprecated and has no effect.", false)]
        public void Awake()
        {
        }
    }
}
