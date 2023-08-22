using CommandLine;
using EnergyAnalyzer.Extensions;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.OptionsManagers.Service;

namespace EnergyAnalyzer
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHost _host;
        private readonly Parser _parser;
        private readonly OptionsManagerService _optionsManagerService;

        public Worker(ILogger<Worker> logger, IHost host, Parser parser, OptionsManagerService optionsManagerService)
        {
            _logger = logger;
            _host = host;
            _parser = parser;
            _optionsManagerService = optionsManagerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var types = _optionsManagerService.GetAllTypesIOptions();

            while (true)
            {
                Console.Write(">");
                var line = Console.ReadLine();
                if (line is null)
                    continue;

                if (line == "stop")
                    break;

                var args = line.CommandLineToArgs();

                await _parser.ParseArguments(args, types.ToArray())
                             .MapResult(
                                async (IOptions options) => await RunOption(options),
                                HandleParseError
                             );
            }

            await _host.StopAsync(stoppingToken);
        }

        private async Task<int> RunOption(IOptions options)
        {
            try
            {
                var optionsManager = _optionsManagerService.CreateOptionsManager(options);
                await optionsManager.ExecuteAsync(options);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return 0;
        }

        private async Task<int> HandleParseError(IEnumerable<Error> errs)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var err in errs)
                Console.WriteLine(err.Tag);
            Console.ForegroundColor = ConsoleColor.White;

            return 1;
        }
    }
}