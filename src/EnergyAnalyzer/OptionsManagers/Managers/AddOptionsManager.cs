using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Models.Data;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class AddOptionsManager : IOptionsManager
    {
        private readonly IWriter _writer;
        private readonly IReader _reader;

        public AddOptionsManager(IWriter writer, IReader reader)
        {
            _writer = writer;
            _reader = reader;
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

            var lastRecord = await _reader.ReadLastOrDefaultAsync();
            eneryMeterEntryItem.Id = lastRecord is null ? 1 : lastRecord.Id + 1;

            await _writer.Write(eneryMeterEntryItem);
        }

        public Type GetOptionsType() => typeof(AddOptions);
    }
}
