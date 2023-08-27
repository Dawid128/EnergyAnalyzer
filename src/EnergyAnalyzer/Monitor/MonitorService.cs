using EnergyAnalyzer.Helpers;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace EnergyAnalyzer.Monitor
{
    internal abstract class MonitorService : IMonitorService
    {
        public const string StartProgramName = "Start Program";
        public const string InsertCommandName = "Insert Command";
        public const string OptionsManagerName = "Options Manager: {0}";

        protected readonly IConfiguration Cofiguration;
        protected readonly ReflectionHelper ReflectionHelper;

        private readonly ActivitySource _activitySource;

        public MonitorService(IConfiguration configuration, ReflectionHelper reflectionHelper)
        {
            Cofiguration = configuration;
            ReflectionHelper = reflectionHelper;
            Init();

            _activitySource = new ActivitySource(nameof(MonitorService));
        }

        protected virtual void Init()
        {
        }

        public virtual Activity? OpenSpan(string name, params (string Name, object Value)[] args)
        {
            var activity = _activitySource.StartActivity(name);
            if (activity is null)
                return null;

            foreach (var arg in args)
                SetTagArg(activity, arg);

            return activity;
        }

        public void SetTagArg(Activity? activity, (string Name, object Value) arg)
        {
            if (activity is null)
                return;

            var metadataVar = ReflectionHelper.GetMetadata(arg.Name, arg.Value);
            foreach (var item in metadataVar)
                activity.SetTag($"arg.{item.Key}", item.Value);
        }

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
