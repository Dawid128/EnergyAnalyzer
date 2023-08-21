using EnergyAnalyzer.Models.Options;

namespace EnergyAnalyzer.OptionsManagers.Interfaces
{
    internal interface IOptionsManager
    {
        public Task ExecuteAsync(IOptions options);
        public Type GetOptionsType();
    }
}
