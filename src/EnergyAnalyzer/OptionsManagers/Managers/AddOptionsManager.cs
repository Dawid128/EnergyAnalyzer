using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Models.Data;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class AddOptionsManager : IOptionsManager, IDisposable
    {
        private readonly DatabaseContext _context;
        private readonly IWriter _writer;

        public AddOptionsManager(IWriter writer, DatabaseContext context)
        {
            _writer = writer;
            _context = context;
        }

        public async Task ExecuteAsync(IOptions options)
        {
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
