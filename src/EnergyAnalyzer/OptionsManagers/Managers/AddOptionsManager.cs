using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Models.Data;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.Monitor;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class AddOptionsManager : IOptionsManager, IDisposable
    {
        private readonly IMonitorService _monitorService;
        private readonly DatabaseContext _context;
        private readonly IWriter _writer;

        public AddOptionsManager(IMonitorService monitorService, IWriter writer, DatabaseContext context)
        {
            _monitorService = monitorService;
            _writer = writer;
            _context = context;
        }

        public async Task ExecuteAsync(IOptions options)
        {
            using var span = _monitorService.OpenSpan(string.Format(MonitorService.OptionsManagerName, "Add"), (nameof(options), options));

            if (options is not AddOptions addEntryOptions)
                throw new ArgumentInvalidTypeException(nameof(options), $"Argument options is not valid type AddOptions");

            var eneryMeterEntryItem = new EnergyMeterEntryItem()
            {
                Date = addEntryOptions.Date,
                Value = addEntryOptions.Value,
            };

            await _writer.CreateAsync(_context, eneryMeterEntryItem);
        }

        public Type GetOptionsType() => typeof(AddOptions);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
