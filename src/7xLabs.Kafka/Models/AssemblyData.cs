using System.Runtime.InteropServices;
using System;
using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka.Models
{
    [ExcludeFromCodeCoverage]
    internal static class AssemblyData
    {
        internal static string MachineName { get; }
        internal static string Kernel { get; }
        internal static string Framework { get; }
        internal static string Version { get; }

        static AssemblyData()
        {
            MachineName = Environment.MachineName;
            Kernel = Environment.OSVersion.VersionString;
            Framework = RuntimeInformation.FrameworkDescription;
            Version = typeof(AssemblyData).Assembly.GetName().Version!.ToString();
        }
    }
}
