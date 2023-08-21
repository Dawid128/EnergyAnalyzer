using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.DataManagers.Interfaces
{
    internal interface IWriter
    {
        Task Write(EnergyMeterEntryItem value);
    }
}
