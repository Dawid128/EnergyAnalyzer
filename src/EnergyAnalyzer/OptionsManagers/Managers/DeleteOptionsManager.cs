using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Helpers;
using EnergyAnalyzer.Models.Data;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.Monitor;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class DeleteOptionsManager : IOptionsManager, IDisposable
    {
        private readonly IMonitorService _monitorService;
        private readonly DeleteOptionsHelper _helper;
        private readonly DatabaseContext _context;
        private readonly IReader _reader;
        private readonly IDeleter _deleter;

        public DeleteOptionsManager(IMonitorService monitorService, DeleteOptionsHelper helper, IReader reader, IDeleter deleter, DatabaseContext context)
        {
            _monitorService = monitorService;
            _helper = helper;
            _reader = reader;
            _deleter = deleter;
            _context = context;
        }

        public async Task ExecuteAsync(IOptions options)
        {
            using var span = _monitorService.OpenSpan(string.Format(MonitorService.OptionsManagerName, "Delete"), (nameof(options), options));

            if (options is not DeleteOptions deleteOptions)
                throw new ArgumentInvalidTypeException(nameof(options), $"Argument options is not valid type DeleteOptions");

            //If All is true, delete all records
            if (deleteOptions.All is true)
            {
                await _deleter.DeleteAllAsync(_context, nameof(_context.EnergyMeterEntryItems));
                return;
            }

            //Else, delete selected Ids 
            var ids = deleteOptions.Ids.SelectMany(_helper.TakeIds);
            if (!ids.Any())
                throw new Exception("Not defined id/s to delete.");

            foreach (var id in ids)
            {
                var item = await _reader.FindAsync<EnergyMeterEntryItem>(_context, id);
                if (item is null)
                    throw new ArgumentInvalidTypeException(nameof(item), $"Not found ID {id}");

                await _deleter.DeleteAsync(_context, item);
            }
        }

        public Type GetOptionsType() => typeof(DeleteOptions);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
