using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Handlers;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class ShowOptionsManager : IOptionsManager
    {
        private readonly ConsoleHandler _consoleHandler;
        private readonly IReader _reader;

        public ShowOptionsManager(IReader reader, ConsoleHandler consoleHandler)
        {
            _consoleHandler = consoleHandler;
            _reader = reader;
        }

        public async Task ExecuteAsync(IOptions options)
        {
            if (options is not ShowOptions showOptions)
                throw new ArgumentInvalidTypeException(nameof(options), $"Argument options is not valid type {nameof(ShowOptions)}");

            var items = await _reader.ReadAllAsync();
            _consoleHandler.WriteObjects(items, new[] { "Id", "Date", "Value" });
        }

        public Type GetOptionsType() => typeof(ShowOptions);
    }
}
