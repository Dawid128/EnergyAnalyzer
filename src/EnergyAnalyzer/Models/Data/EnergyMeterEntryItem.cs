using EnergyAnalyzer.Models.Attributes;

namespace EnergyAnalyzer.Models.Data
{
    [Serializable]
    internal class EnergyMeterEntryItem : Item
    {
        [ItemProperty]
        public DateTime Date { get; set; }

        [ItemProperty]
        public int Value { get; set; }
    }
}
