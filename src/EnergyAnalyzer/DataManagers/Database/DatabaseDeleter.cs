using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Models.Data;
using System.Data.Entity;

namespace EnergyAnalyzer.DataManagers.Database
{
    internal class DatabaseDeleter : IDeleter
    {
        public async Task DeleteAsync<T>(DatabaseContext context, IList<T> items) where T : Item, new()
        {
            foreach (var item in items)
                context.Entry(item).State = EntityState.Deleted;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync(DatabaseContext context, string tableName)
        {
            await context.Database.ExecuteSqlCommandAsync($"TRUNCATE TABLE [{tableName}]");
        }
    }
}
