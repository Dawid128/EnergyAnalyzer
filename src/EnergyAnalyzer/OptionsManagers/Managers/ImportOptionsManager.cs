using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Models.Data;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.Monitor;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class ImportOptionsManager(IMonitorService monitorService, IWriter writer, DatabaseContext context, IImporter importer) : IOptionsManager, IDisposable
    {
        private readonly IMonitorService _monitorService = monitorService;
        private readonly DatabaseContext _context = context;
        private readonly IWriter _writer = writer;
        private readonly IImporter _importer = importer;

        public async Task ExecuteAsync(IOptions options)
        {
            using var span = _monitorService.OpenSpan(string.Format(MonitorService.OptionsManagerName, "Import"), (nameof(options), options));

            if (options is not ImportOptions importOptions)
                throw new ArgumentInvalidTypeException(nameof(options), $"Argument options is not valid type ImportOptions");

            var importEntries = await _importer.ImportAsync<EnergyMeterEntryItem>(importOptions.Path);
            if (importEntries is null || !importEntries.Any())
            {
                throw new Exception("Not found entries to import");
            }

            foreach (var importEntry in importEntries)
            {
                await _writer.CreateAsync(_context, importEntry);
            }
        }

        public Type GetOptionsType() => typeof(ImportOptions);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
