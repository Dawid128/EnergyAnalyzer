using EnergyAnalyzer.Database;
using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.DataManagers.Interfaces
{
    internal interface IWriter
    {
        Task CreateAsync<T>(DatabaseContext context, T value) where T : Item, new();
    }
}
