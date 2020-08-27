using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Runtime.InteropServices;
using XRStats = UnityEngine.XR.Provider.XRStats;

namespace Unity.XR.Oculus
{
    static class Performance
    {
        /// <summary>
        /// Set the CPU Level for the device
        /// </summary>
        /// <param name="cpuLevel">
        /// Allowable values are integers in the range 0 - 3. A value of 0 is the lowest performance level but is the most power efficient.
        /// </param>
        [DllImport("OculusXRPlugin", CharSet = CharSet.Auto)]
        public static extern void SetCPULevel(int cpuLevel);

        /// <summary>
        /// Set the GPU performance level 
        /// </summary>
        /// <param name="gpuLevel"> 
        /// Allowable values are integers in the range 0 - 3. A value of 0 is the lowest performance level but is the most power efficient.
        /// </param>
        [DllImport("OculusXRPlugin", CharSet = CharSet.Auto)]
        public static extern void SetGPULevel(int gpuLevel);
    }

    public class Stats
    {
        private static IntegratedSubsystem m_Display;
        private static string m_PluginVersion = string.Empty;

        [DllImport("OculusXRPlugin", CharSet=CharSet.Auto)]
        private static extern void GetOVRPVersion(byte[] version);


        /// <summary>
        /// Gets the version of OVRPlugin currently in use. Format: "major.minor.release"
        /// </summary>
        public static string PluginVersion
        {
            get
            {
                if (string.Equals(string.Empty, m_PluginVersion))
                {
                    byte[] buf = new byte[256];
                    GetOVRPVersion(buf);
                    var end = Array.IndexOf<byte>(buf, 0);
                    m_PluginVersion = System.Text.Encoding.ASCII.GetString(buf, 0, end);
                }
                return m_PluginVersion;
            }
        }

        /// <summary>
        /// Provides performance statistics useful for adaptive performance systems. Not every stat is supported on every Oculus platform and will always return a value of 0 if unsupported.
        /// </summary>
        public static class AdaptivePerformance
        {
            /// <summary>
            /// Reports the time the application spent on the GPU last frame in seconds.
            /// </summary>
            public static float GPUAppTime
            {
                get
                {
                    float val;
                    ((XRDisplaySubsystem) GetOculusDisplaySubsystem()).TryGetAppGPUTimeLastFrame(out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the time the compositor spent on the GPU last frame in seconds. 
            /// </summary>
            public static float GPUCompositorTime
            {
                get
                {
                    float val;
                    ((XRDisplaySubsystem) GetOculusDisplaySubsystem()).TryGetCompositorGPUTimeLastFrame(out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the latency from when the last predicted tracking information was queried by the application to the moment the middle scaline of the target frame is illuminated on the display.
            /// </summary>
            public static float MotionToPhoton
            {
                get
                {
                    float val;
                    ((XRDisplaySubsystem) GetOculusDisplaySubsystem()).TryGetMotionToPhoton(out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the display's refresh rate in frames per second
            /// </summary>
            public static float RefreshRate
            {
                get
                {
                    float val;
                    ((XRDisplaySubsystem) GetOculusDisplaySubsystem()).TryGetDisplayRefreshRate(out val);
                    return val;
                }
            }

            /// <summary>
            /// Gets the current battery temperature in degrees Celsius.
            /// </summary>
            public static float BatteryTemp
            {
                get
                {
                    float batteryTemp;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "batteryTemperature", out batteryTemp);
                    return batteryTemp;
                }
            }

            /// <summary>
            /// Gets the current available battery charge, ranging from 0.0 (empty) to 1.0 (full).
            /// </summary>
            public static float BatteryLevel
            {
                get
                {
                    float batteryLevel;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "batteryLevel", out batteryLevel);
                    return batteryLevel;
                }
            }

            /// <summary>
            /// If true, the system is running in a reduced performance mode to save power.
            /// </summary>
            public static bool PowerSavingMode
            {
                get
                {
                    float powerSavingMode;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "powerSavingMode", out powerSavingMode);
                    return !(powerSavingMode == 0.0f);
                }
            }

            /// <summary>
            /// Returns the recommended amount to scale GPU work in order to maintain framerate.
            /// Can be used to adjust viewportScale and textureScale
            /// </summary>
            public static float AdaptivePerformanceScale
            {
                get
                {
                    float performanceScale;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "adaptivePerformanceScale", out performanceScale);
                    return performanceScale;
                }
            }

            /// <summary>
            /// Gets the current CPU performance level, integer in the range 0 - 3.
            /// </summary>
            public static int CPULevel
            {
                get
                {
                    float cpuLevel;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "cpuLevel", out cpuLevel);
                    return (int) cpuLevel;
                }
            }

            /// <summary>
            /// Gets the current GPU performance level, integer in the range 0 - 3.
            /// </summary>
            public static int GPULevel
            {
                get
                {
                    float gpuLevel;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "gpuLevel", out gpuLevel);
                    return (int) gpuLevel;
                }
            }
        }

        /// <summary>
        /// Provides additional perf metrics. These stats will not be tracked unless the user makes a PerfMetrics.EnablePerfMetrics(true) method call. Not every stat is supported on every Oculus platform and will always return a value of 0 if unsupported.
        /// </summary>
        public static class PerfMetrics
        {
            /// <summary>
            /// Reports the time the application spent on the CPU last frame in seconds.
            /// </summary>
            public static float AppCPUTime
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "perfmetrics.appcputime", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the time the application spen on the GPU last frame in seconds.
            /// </summary>
            public static float AppGPUTime
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "perfmetrics.appgputime", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the time the compositor spent on the CPU last frame in seconds.
            /// </summary>
            public static float CompositorCPUTime
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "perfmetrics.compositorcputime", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the time the compositor spent on the GPU last frame in seconds.
            /// </summary>
            public static float CompositorGPUTime
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "perfmetrics.compositorgputime", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the GPU utilization as a value from 0.0 - 1.0.
            /// </summary>
            public static float GPUUtilization
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "perfmetrics.gpuutil", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the average CPU utilization as a value from 0.0 - 1.0.
            /// </summary>
            public static float CPUUtilizationAverage
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "perfmetrics.cpuutilavg", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the worst CPU utilization as a value from 0.0 - 1.0.
            /// </summary>
            public static float CPUUtilizationWorst
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "perfmetrics.cpuutilworst", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the CPU clock frequency
            /// </summary>
            public static float CPUClockFrequency
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "perfmetrics.cpuclockfreq", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the GPU clock frequency
            /// </summary>
            public static float GPUClockFrequency
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "perfmetrics.gpuclockfreq", out val);
                    return val;
                }
            }

            /// <summary>
            /// Enable or disable provider tracking perf metrics. Perf metrics are disabled by default.
            /// </summary>
            [DllImport("OculusXRPlugin", CharSet = CharSet.Auto)]
            public static extern void EnablePerfMetrics(bool enable);
        }

        /// <summary>
        /// Provides additional application metrics. These metrics will not be tracked unless the user makes a AppMetrics.EnableAppMetrics(true) method call. Not every stat is supported on every Oculus platform and will always return a value of 0 if unsupported.
        /// </summary>
        public static class AppMetrics
        {
            public static float AppQueueAheadTime
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "appstats.appqueueahead", out val);
                    return val;
                }
            }

            public static float AppCPUElapsedTime
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "appstats.cpuelapsedtime", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the number of frames dropped by the compositor.
            /// </summary>
            public static float CompositorDroppedFrames
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "appstats.compositordroppedframes", out val);
                    return val;
                }
            }

            public static float CompositorLatency
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "appstats.compositorlatency", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the time the compositor spent on the CPU last frame in seconds.
            /// </summary>
            public static float CompositorCPUTime
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "appstats.compositorcputime", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the total time from beginning of the main thread until compositor frame submission.
            /// </summary>
            public static float CPUStartToGPUEnd
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "appstats.compositorcpustartgpuendelapsedtime", out val);
                    return val;
                }
            }

            /// <summary>
            /// Reports the total time from compositor frame submission until the frame sync point.
            /// </summary>
            public static float GPUEndToVsync
            {
                get
                {
                    float val;
                    XRStats.TryGetStat(GetOculusDisplaySubsystem(), "appstats.compositorgpuendtovsyncelapsedtime", out val);
                    return val;
                }
            }

            [DllImport("OculusXRPlugin", CharSet = CharSet.Auto)]
            public static extern void EnableAppMetrics(bool enable);
        }


        private static IntegratedSubsystem GetOculusDisplaySubsystem()
        {
            if (m_Display != null)
                return m_Display;
            List<XRDisplaySubsystem> displays = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances(displays);

            foreach (XRDisplaySubsystem xrDisplaySubsystem in displays)
            {
                if (xrDisplaySubsystem.SubsystemDescriptor.id == "oculus display" && xrDisplaySubsystem.running)
                {
                    m_Display = xrDisplaySubsystem;
                    return m_Display;
                }
            }
            Debug.LogError("No active Oculus display subsystem was found.");
            return m_Display;
        }
    }
}


