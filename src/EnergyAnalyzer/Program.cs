using CommandLine;
using EnergyAnalyzer;
using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Handlers;
using EnergyAnalyzer.OptionsManagers.Managers;
using EnergyAnalyzer.OptionsManagers.Service;
using System.Globalization;
using EnergyAnalyzer.Monitor;
using EnergyAnalyzer.Helpers;
using EnergyAnalyzer.Services;

IHost host = Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration(ConfigureAppConfiguration)
                 .ConfigureLogging(ConfigureLogging)
                 .ConfigureServices(ConfugureServices)
                 .Build();

host.Run();

static void ConfigureAppConfiguration(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory());
    builder.AddJsonFile("appsettings.json", false);
    builder.Build();
}

static void ConfigureLogging(ILoggingBuilder loggingBuilder)
{
    loggingBuilder.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
}

//Split all add as separate group. 
static void ConfugureServices(HostBuilderContext builder, IServiceCollection services)
{
    //Add Main Application
    services.AddHostedService<Worker>();

    //Add Worker Componets
    services.AddSingleton(x => new Parser(ConfigureParser));
    services.AddSingleton<OptionsManagerService>();

    //Add Options Managers & Helpers
    services.AddTransient<AddOptionsManager>();
    services.AddTransient<DeleteOptionsManager>();
    services.AddSingleton<DeleteOptionsHelper>();
    services.AddTransient<ShowOptionsManager>();
    services.AddTransient<CreateAnalysisOptionsManager>();

    //Add Analysis Services
    services.AddSingleton<DailyEnergyConsumptions>();

    //Add Data Manager
    services.AddSingleton<IWriter, DatabaseWriter>();
    services.AddSingleton<IDeleter, DatabaseDeleter>();
    services.AddSingleton<IReader, DatabaseReader>();

    //Add Database Context
    services.AddTransient<DatabaseContext>();

    //Add Handlers
    services.AddSingleton<ConsoleHandler>();

    //Add Helpers
    services.AddSingleton<ReflectionHelper>();

    //Add Monitor
    var service = builder.Configuration.GetSection("Monitor").GetValue<string>("Service");
    if (service == "Sentry")
        services.AddSingleton<IMonitorService, SentryMonitorService>();
    else
        services.AddSingleton<IMonitorService, NullMonitorService>();
}

static void ConfigureParser(ParserSettings settings)
{
    settings.ParsingCulture = new CultureInfo("pl-pl");
    settings.HelpWriter = Console.Error;
    settings.AutoHelp = true;   
    settings.AutoVersion = true;
    settings.CaseInsensitiveEnumValues = true;
    settings.CaseSensitive = true;
    settings.IgnoreUnknownArguments = true;
}