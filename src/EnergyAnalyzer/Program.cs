using CommandLine;
using EnergyAnalyzer;
using EnergyAnalyzer.DataManagers;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Handlers;
using EnergyAnalyzer.OptionsManagers.Interfaces;
using EnergyAnalyzer.OptionsManagers.Managers;
using System.Globalization;

IHost host = Host.CreateDefaultBuilder(args)
                 .ConfigureLogging(ConfigureLogging)
                 .ConfigureServices(ConfugureServices)
                 .Build();

host.Run();

static void ConfigureLogging(ILoggingBuilder loggingBuilder)
{
    loggingBuilder.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
}

static void ConfugureServices(IServiceCollection services)
{
    //Add Main Application
    services.AddHostedService<Worker>();

    //Add Parser
    services.AddSingleton(x => new Parser(ConfigureParser));

    //Add Data Manager
    services.AddSingleton<IDataManager, FileCSVDataManager>(x => new FileCSVDataManager(@"C:\ProgramData\EnergyAnalyzer\CSV"));
    services.AddSingleton<IWriter>(x => x.GetRequiredService<IDataManager>());
    services.AddSingleton<IDeleter>(x => x.GetRequiredService<IDataManager>());
    services.AddSingleton<IReader>(x => x.GetRequiredService<IDataManager>());

    //Add Options Managers
    services.AddSingleton<IOptionsManager, AddOptionsManager>();
    services.AddSingleton<IOptionsManager, DeleteOptionsManager>();
    services.AddSingleton<IOptionsManager, ShowOptionsManager>();

    //Add Handlers
    services.AddSingleton<ConsoleHandler>();
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