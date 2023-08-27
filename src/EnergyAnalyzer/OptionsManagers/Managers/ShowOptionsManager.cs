using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Handlers;
using EnergyAnalyzer.Models.Data;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.Monitor;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class ShowOptionsManager : IOptionsManager, IDisposable
    {
        private readonly IMonitorService _monitorService;
        private readonly DatabaseContext _context;
        private readonly ConsoleHandler _consoleHandler;
        private readonly IReader _reader;

        public ShowOptionsManager(IMonitorService monitorService, IReader reader, ConsoleHandler consoleHandler, DatabaseContext context)
        {
            _monitorService = monitorService;
            _consoleHandler = consoleHandler;
            _reader = reader;
            _context = context;
        }

        public async Task ExecuteAsync(IOptions options)
        {
            using var span = _monitorService.OpenSpan(string.Format(MonitorService.OptionsManagerName, "Show"), (nameof(options), options));

            if (options is not ShowOptions showOptions)
                throw new ArgumentInvalidTypeException(nameof(options), $"Argument options is not valid type {nameof(ShowOptions)}");

            var items = await _reader.ReadAllAsync<EnergyMeterEntryItem>(_context);

            _consoleHandler.WriteObjects(items, new[] { "Id", "Date", "Value" });
        }

        public Type GetOptionsType() => typeof(ShowOptions);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
