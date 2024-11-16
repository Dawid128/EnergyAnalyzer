using EnergyAnalyzer.Models.Attributes;

namespace EnergyAnalyzer.Models.Data
{
    internal class DailyEnergyConsumptionItem : Item
    {
        [ItemProperty]
        public DateOnly Date { get; set; }

        [ItemProperty]
        public double Value { get; set; }
    }
}
