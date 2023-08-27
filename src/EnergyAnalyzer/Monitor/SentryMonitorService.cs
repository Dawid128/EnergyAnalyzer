using EnergyAnalyzer.Helpers;
﻿using OpenTelemetry;
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
        public SentryMonitorService(IConfiguration configuration, ReflectionHelper reflectionHelper) : base(configuration, reflectionHelper) 
        { 

        }

        protected override void Init()
        {
            SentrySdk.Init(ConfigureSentry);
            //SentrySdk.ConfigureScope(ConfigureScope);

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
            //options.AddTransactionProcessor(new StatusTransactionProcessor());
        }

        //private void ConfigureScope(Scope scope)
        //{
        //    scope.AddAttachment(@"C:\Users\DCz\source\repos\ProjectsDCz\2023\EnergyAnalyzer\src\EnergyAnalyzer\bin\Debug\net7.0\appsettings.json");
        //}

        public override void LogException(Activity? activity, Exception exception)
        {
            //SentrySdk.CaptureException(exception, scope => scope.AddAttachment(@"C:\Users\DCz\source\repos\ProjectsDCz\2023\EnergyAnalyzer\src\EnergyAnalyzer\bin\Debug\net7.0\appsettings.json"));
            SentrySdk.CaptureException(exception);

            base.LogException(activity, exception);
        }
    }

    //Sentry wspiera Tracing (Transakcja + Spans)
    //Sentry wspiera Tags 
    //Sentry wspiera Error jako oddzielne elementy, ale nie łączy tego ze Span
    //Sentry wspiera Załączniki dla Errors & Messages

    //Sentry może odrzucać tracing na podstawie danych (np. status Uknown)

    //Sentry nie wspiera logowania jak Jaeger 
    //Sentry ma problem z intrumentami ASP .NET Core (Do rozwiązania)? 
    //Sentry dla .NET oparte na telemetri potrafi mniej niż bez telemetri. 
    //--Ustawianie Nazwy Transakcji & Operation -> telemetry ustawia taką samą wartość dla Transaction & Operation & Description
    //--Ustawienie Status -> telemetry wspiera tylko OK & Uknown

}
