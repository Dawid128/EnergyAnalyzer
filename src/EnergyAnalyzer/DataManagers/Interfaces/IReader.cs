using EnergyAnalyzer.Database;
using EnergyAnalyzer.Models.Data;
using System.Linq.Expressions;

namespace EnergyAnalyzer.DataManagers.Interfaces
{
    internal interface IReader
    {
        Task<List<T>> ReadAllAsync<T>(DatabaseContext context) where T : Item, new();
        Task<List<T>> ReadAllAsync<T>(DatabaseContext context, Expression<Func<T, bool>> predicate) where T : Item, new();
        Task<T> FindAsync<T>(DatabaseContext context, int id) where T : Item, new();
    }
}
