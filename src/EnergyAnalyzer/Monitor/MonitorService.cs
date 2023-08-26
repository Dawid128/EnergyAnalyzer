using OpenTelemetry.Trace;
using System.Diagnostics;

namespace EnergyAnalyzer.Monitor
{
    internal abstract class MonitorService : IMonitorService
    {
        protected readonly IConfiguration Cofiguration;

        private readonly ActivitySource _activitySource;

        public MonitorService(IConfiguration configuration)
        {
            Cofiguration = configuration;
            Init();

            _activitySource = new ActivitySource(nameof(MonitorService));
        }

        protected virtual void Init()
        {
        }

        public virtual Activity? OpenSpan(string name) => _activitySource.StartActivity(name);

        public virtual void LogException(Exception exception) => LogException(Activity.Current, exception);

        public virtual void LogException(Activity? activity, Exception exception)
        {
            //A może powinienem wymagać NOT NULL ? 
            if (activity is null)
                return;

            activity.RecordException(exception);
            activity.SetStatus(Status.Error.WithDescription(exception.Message));
        }
    }
}
