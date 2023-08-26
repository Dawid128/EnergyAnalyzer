using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sentry;
using Sentry.Extensibility;
using Sentry.OpenTelemetry;
using System.Diagnostics;

namespace EnergyAnalyzer.Monitor
{
    internal class SentryMonitorService : MonitorService, IMonitorService
    {
        public SentryMonitorService(IConfiguration configuration) : base(configuration) 
        { 

        }

        protected override void Init()
        {
            SentrySdk.Init(Configure);

            Sdk.CreateTracerProviderBuilder()
               .AddSource(nameof(MonitorService))
               .ConfigureResource(configure => configure.AddService("WindowsService"))
               .AddSentry()
               .Build();
        }

        private void Configure(SentryOptions options)
        {
            options.Dsn = Cofiguration.GetSection("Monitor:SentryOptions").GetValue<string>("Dsn");
            options.Environment = Cofiguration.GetSection("Monitor:SentryOptions").GetValue<string>("Environment");
            options.SendDefaultPii = Cofiguration.GetSection("Monitor:SentryOptions").GetValue<bool>("SendDefaultPii", false);
            options.Debug = Cofiguration.GetSection("Monitor:SentryOptions").GetValue<bool>("Debug", false);
            options.DiagnosticLevel = Cofiguration.GetSection("Monitor:SentryOptions").GetValue<SentryLevel>("DiagnosticLevel", SentryLevel.Info);
            options.EnableTracing = Cofiguration.GetSection("Monitor:SentryOptions").GetValue<bool>("EnableTracing", false);
            options.TracesSampleRate = Cofiguration.GetSection("Monitor:SentryOptions").GetValue<double>("TracesSampleRate", 1.0);

            //options.StackTraceMode = StackTraceMode.Enhanced;
            options.AttachStacktrace = true;
            options.AutoSessionTracking = true;
            options.IsGlobalModeEnabled = true;

            options.UseOpenTelemetry();
            //options.AddTransactionProcessor(new StatusTransactionProcessor());
        }

        public override void LogException(Activity? activity, Exception exception)
        {
            SentrySdk.CaptureException(exception);

            base.LogException(activity, exception);
        }
    }

    //internal class StatusTransactionProcessor : ISentryTransactionProcessor
    //{
    //    public Transaction? Process(Transaction transaction)
    //    {
    //        return transaction;
    //    }
    //}
}
