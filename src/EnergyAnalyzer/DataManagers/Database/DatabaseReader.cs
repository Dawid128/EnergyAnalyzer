using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Models.Data;
using System.Data.Entity;
using System.Linq.Expressions;

namespace EnergyAnalyzer.DataManagers.Database
{
    internal class DatabaseReader : IReader
    {
        public async Task<List<T>> ReadAllAsync<T>(DatabaseContext context) where T : Item, new()
        {
            var result = await context.Set<T>().ToListAsync();

            return result;
        }

        public async Task<List<T>> ReadAllAsync<T>(DatabaseContext context, Expression<Func<T, bool>> predicate) where T : Item, new()
        {
            var result = await context.Set<T>().Where(predicate).ToListAsync();

            return result;
        }

        public async Task<T> FindAsync<T>(DatabaseContext context, int id) where T : Item, new()
        {
            var result = await context.Set<T>().FindAsync(id);

            return result;
        }
    }
}
