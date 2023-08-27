using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Models.Data;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.Monitor;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class DeleteOptionsManager : IOptionsManager, IDisposable
    {
        private readonly IMonitorService _monitorService;
        private readonly DatabaseContext _context;
        private readonly IReader _reader;
        private readonly IDeleter _deleter;

        public DeleteOptionsManager(IMonitorService monitorService, IReader reader, IDeleter deleter, DatabaseContext context)
        {
            _monitorService = monitorService;
            _reader = reader;
            _deleter = deleter;
            _context = context;
        }

        public async Task ExecuteAsync(IOptions options)
        {
            using var span = _monitorService.OpenSpan(string.Format(MonitorService.OptionsManagerName, "Delete"), (nameof(options), options));

            if (options is not DeleteOptions deleteOptions)
                throw new ArgumentInvalidTypeException(nameof(options), $"Argument options is not valid type DeleteOptions");

            var item = await _reader.FindAsync<EnergyMeterEntryItem>(_context, deleteOptions.Id);
            if (item is null)
                throw new ArgumentInvalidTypeException(nameof(item), $"Not found ID {deleteOptions.Id}");

            await _deleter.DeleteAsync(_context, item);
        }

        public Type GetOptionsType() => typeof(DeleteOptions);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
