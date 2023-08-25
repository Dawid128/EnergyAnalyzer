using EnergyAnalyzer.Database;
using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.DataManagers.Interfaces
{
    internal interface IDeleter
    {
        public Task DeleteAsync<T>(DatabaseContext context, T item) where T : Item, new();
    }
}
