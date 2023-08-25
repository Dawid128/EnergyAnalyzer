using EnergyAnalyzer.Models.Data;
using System.Data.Entity;

namespace EnergyAnalyzer.Database
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<EnergyMeterEntryItem> EnergyMeterEntryItems { get; set; } = null!;
    }
}
