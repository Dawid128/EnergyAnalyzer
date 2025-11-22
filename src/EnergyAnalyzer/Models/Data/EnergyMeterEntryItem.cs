using EnergyAnalyzer.Models.Attributes;

namespace EnergyAnalyzer.Models.Data
{
    [Serializable]
    internal class EnergyMeterEntryItem : Item
    {
        [ItemProperty]
        [ItemImport(ImportOrder = 1)]
        public DateTime Date { get; set; }

        [ItemProperty]
        [ItemImport(ImportOrder = 2)]
        public int Value { get; set; }
    }
}
