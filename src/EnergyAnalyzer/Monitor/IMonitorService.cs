using System.Diagnostics;

namespace EnergyAnalyzer.Monitor
{
    internal interface IMonitorService
    {
        Activity? OpenSpan(string name);
        void LogException(Exception exception);
        void LogException(Activity? activity, Exception exception);
    }
}
