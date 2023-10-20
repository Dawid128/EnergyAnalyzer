using EnergyAnalyzer.Helpers;
using System.Diagnostics;

namespace EnergyAnalyzer.Monitor
{
    internal class NullMonitorService : MonitorService, IMonitorService
    {
        public NullMonitorService(IConfiguration configuration, ReflectionHelper reflectionHelper) : base(configuration, reflectionHelper)
        {

        }

        protected override void Init() { }

        public override void LogException(Activity? activity, Exception exception) { }

        public override void LogInfo(Activity? activity, string message) { }
    }
}
