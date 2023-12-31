﻿using EnergyAnalyzer.Helpers;
﻿using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sentry;
using Sentry.OpenTelemetry;
using System.Diagnostics;

namespace EnergyAnalyzer.Monitor
{
    internal class SentryMonitorService : MonitorService, IMonitorService
    {
        public SentryMonitorService(IConfiguration configuration, ReflectionHelper reflectionHelper) : base(configuration, reflectionHelper)
        {

        }

        protected override void Init()
        {
            SentrySdk.Init(ConfigureSentry);

            Sdk.CreateTracerProviderBuilder()
               .AddSource(nameof(MonitorService))
               .ConfigureResource(configure => configure.AddService("WindowsService"))
               .AddSentry()
               .Build();
        }

        private void ConfigureSentry(SentryOptions options)
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
        }

        public override void LogException(Activity? activity, Exception exception)
        {
            base.LogException(activity, exception);

            SentrySdk.CaptureException(exception);
        }

        public override void LogInfo(Activity? activity, string message)
        {
            base.LogInfo(activity, message);

            SentrySdk.CaptureMessage(message, SentryLevel.Info);
        }
    }

    //Sentry wspiera Tracing (Transakcja + Spans)
    //Sentry wspiera Tags 
    //Sentry wspiera Error jako oddzielne elementy, ale nie łączy tego ze Span
    //Sentry wspiera Załączniki dla Errors & Messages

    //Sentry może odrzucać tracing na podstawie danych (np. status Uknown)

    //Sentry nie wspiera Status in Message -> wyświetlane jest default. 
    //Sentry nie wspiera logowania jak Jaeger 
    //Sentry ma problem z intrumentami ASP .NET Core (Do rozwiązania)? 
    //Sentry dla .NET oparte na telemetri potrafi mniej niż bez telemetri. 
    //--Ustawianie Nazwy Transakcji & Operation -> telemetry ustawia taką samą wartość dla Transaction & Operation & Description
    //--Ustawienie Status -> telemetry wspiera tylko OK & Uknown

    //Istnieje różnica że Jaeger wysyła SPAN w czasie rzeczywistym (możliwa jest utrata SPAN gdy przekroczy rozmiar) 
    //Natomiast Sentry wysyła gotową transakcje (cała transakcja może zostać utracona jeżeli przekroczy rozmiar)

    //Jaeger wspiera wyświetlanie JSON z formatem, Sentry tego nie wspiera.

    //Jaeger wspiera tagi dla 1 span (root), Sentry traktuje root inaczej.
}
