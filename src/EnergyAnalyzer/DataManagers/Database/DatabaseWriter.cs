using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.DataManagers.Database
{
    internal class DatabaseWriter : IWriter
    {
        public async Task CreateAsync<T>(DatabaseContext context, T value) where T : Item, new()
        {
            var list = context.Set<T>();
            list.Add(value);

            await context.SaveChangesAsync();
        }
    }
}
