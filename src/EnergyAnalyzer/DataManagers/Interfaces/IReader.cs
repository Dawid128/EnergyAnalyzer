using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.DataManagers.Interfaces
{
    internal interface IReader
    {
        Task<EnergyMeterEntryItem?> ReadLastOrDefaultAsync();
        Task<List<EnergyMeterEntryItem>> ReadAllAsync();
        Task<EnergyMeterEntryItem?> FindAsync(int id);
    }
}
