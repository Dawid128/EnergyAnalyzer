﻿using EnergyAnalyzer.Helpers;
using Newtonsoft.Json;
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

            var valueStr = JsonConvert.SerializeObject(arg.Value, Formatting.Indented);
            activity.SetTag($"arg.{arg.Name}", valueStr);
        }

        public virtual void LogException(Exception exception) => LogException(Activity.Current, exception);
        public virtual void LogException(Activity? activity, Exception exception)
        {
            //A może powinienem wymagać NOT NULL ? 
            if (activity is null)
                return;

            activity.RecordException(exception);
            activity.SetStatus(Status.Error);
        }

        public virtual void LogInfo(string message) => LogInfo(Activity.Current, message);
        public virtual void LogInfo(Activity? activity, string message)
        {
            if (activity is null)
                return;

            var tags = new Dictionary<string, object?>
            {
                { "Time",  DateTime.Now},
                { "Level", "Information" },
                { "Message", message }
            };

            activity.AddEvent(new ActivityEvent("A", DateTime.Now, new ActivityTagsCollection(tags)));
        }
    }
}
