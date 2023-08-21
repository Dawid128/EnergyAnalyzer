using CommandLine;
using EnergyAnalyzer.Extensions;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHost _host;
        private readonly Parser _parser;

        private readonly Dictionary<Type, Func<IOptions, Task>> _actions = new();

        public Worker(ILogger<Worker> logger, IHost host, Parser parser, IEnumerable<IOptionsManager> optionsManagers)
        {
            _logger = logger;
            _host = host;
            _parser = parser;

            InitializeActions(optionsManagers);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Console.Write(">");
                var line = Console.ReadLine();
                if (line is null)
                    continue;

                if (line == "stop")
                    break;

                var args = line.CommandLineToArgs();

                await _parser.ParseArguments(args, _actions.Select(x => x.Key).ToArray())
                             .MapResult(
                                async (IOptions options) => await RunOption(options),
                                HandleParseError
                             );
            }

            await _host.StopAsync(stoppingToken);
        }

        private void InitializeActions(IEnumerable<IOptionsManager> optionsManagers)
        {
            foreach (var optionsManager in optionsManagers)
            {
                var type = optionsManager.GetOptionsType();
                var action = new Func<IOptions, Task>(x => optionsManager.ExecuteAsync(x));
                _actions.Add(type, action);
            }
        }

        private async Task<int> RunOption(IOptions options)
        {
            try
            {
                var action = _actions.First(x => x.Key == options.GetType());
                await action.Value(options);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return 0;
        }

        static async Task<int> HandleParseError(IEnumerable<Error> errs)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var err in errs)
                Console.WriteLine(err.Tag);
            Console.ForegroundColor = ConsoleColor.White;

            return 1;
        }
    }
}