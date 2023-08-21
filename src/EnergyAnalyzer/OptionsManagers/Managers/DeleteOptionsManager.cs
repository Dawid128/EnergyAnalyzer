using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class DeleteOptionsManager : IOptionsManager
    {
        private readonly IReader _reader;
        private readonly IDeleter _deleter;

        public DeleteOptionsManager(IReader reader, IDeleter deleter)
        {
            _reader = reader;
            _deleter = deleter;
        }

        public async Task ExecuteAsync(IOptions options)
        {
            if (options is not DeleteOptions deleteOptions)
                throw new ArgumentInvalidTypeException(nameof(options), $"Argument options is not valid type DeleteOptions");

            var item = await _reader.FindAsync(deleteOptions.Id);
            if (item is null)
                throw new ArgumentInvalidTypeException(nameof(item), $"Not found ID {deleteOptions.Id}");

            await _deleter.Delete(item);
        }

        public Type GetOptionsType() => typeof(DeleteOptions);
    }
}
