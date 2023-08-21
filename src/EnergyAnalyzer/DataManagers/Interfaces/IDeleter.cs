
using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.DataManagers.Interfaces
{
    internal interface IDeleter
    {
        public Task Delete(EnergyMeterEntryItem item);
    }
}
