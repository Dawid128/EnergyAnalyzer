using EnergyAnalyzer.Database;
using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.DataManagers.Interfaces
{
    internal interface IDeleter
    {
        public Task DeleteAsync<T>(DatabaseContext context, T item) where T : Item, new() => DeleteAsync(context, new[] { item });
        public Task DeleteAsync<T>(DatabaseContext context, IList<T> items) where T : Item, new();
        public Task DeleteAllAsync(DatabaseContext context, string tableName);
    }
}
