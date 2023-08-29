using System.Diagnostics;

namespace EnergyAnalyzer.Monitor
{
    internal interface IMonitorService
    {
        Activity? OpenSpan(string name, params (string Name, object Value)[] args);

        void SetTagArg(Activity? activity, (string Name, object Value) arg);

        void LogException(Exception exception);
        void LogException(Activity? activity, Exception exception);

        void LogInfo(string message);
        void LogInfo(Activity? activity, string message);
    }
}
