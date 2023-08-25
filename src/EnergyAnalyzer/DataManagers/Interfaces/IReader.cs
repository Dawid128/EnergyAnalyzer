using EnergyAnalyzer.Database;
using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.DataManagers.Interfaces
{
    internal interface IReader
    {
        Task<List<T>> ReadAllAsync<T>(DatabaseContext context) where T : Item, new();
        Task<T> FindAsync<T>(DatabaseContext context, int id) where T : Item, new();
    }
}
