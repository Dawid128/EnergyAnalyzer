using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Models.Data;
using System.Data.Entity;

namespace EnergyAnalyzer.DataManagers.Database
{
    internal class DatabaseDeleter : IDeleter
    {
        public async Task DeleteAsync<T>(DatabaseContext context, T item) where T : Item, new()
        {
            context.Entry(item).State = EntityState.Deleted;

            await context.SaveChangesAsync();
        }
    }
}
