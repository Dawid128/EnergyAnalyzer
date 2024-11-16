using EnergyAnalyzer.Extensions;
using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.Services
{
    internal class DailyEnergyConsumptions
    {
        public List<DailyEnergyConsumptionItem> Calculate(IList<EnergyMeterEntryItem> items)
        {
            //Validation
            if (items.Count < 2)
                throw new ArgumentOutOfRangeException(nameof(items), "input items contains less than 2 records");

            if (!items.IsOrderByAscending(x => x.Value))
                throw new ArgumentOutOfRangeException(nameof(items), "input items don't have sort by ascending");

            //Work
            var sData = SplitData(items);
            var cData = CalculateData(sData);
            var result = ConvertToResult(cData);

            return result;
        }

        private static List<(double TotalValue, List<(DateOnly Date, int PartSeconds)> Records)> SplitData(IList<EnergyMeterEntryItem> items)
        {
            List<(double TotalValue, List<(DateOnly Date, int PartSeconds)> Records)> result = new();

            var pointerDateTime = items.First().Date;
            for (int i = 0; i <= items.Count - 2; i++)
            {
                var nextDateTime = items[i + 1].Date;
                var diffValue = items[i + 1].Value - items[i].Value;

                result.Add((diffValue, new List<(DateOnly Date, int PartSeconds)>()));

                while (pointerDateTime < nextDateTime)
                {
                    var nextPointerDateTime = nextDateTime.IsSameDay(pointerDateTime)
                                                          ? nextDateTime
                                                          : pointerDateTime.Date.AddDays(1);

                    var partSeconds = (int)(nextPointerDateTime - pointerDateTime).TotalSeconds;
                    result.Last().Records.Add((pointerDateTime.ToDateOnly(), partSeconds));
                    pointerDateTime = nextPointerDateTime;
                }
            }

            return result;
        }

        private static List<(DateOnly Date, double PartValue)> CalculateData(List<(double TotalValue, List<(DateOnly Date, int PartSeconds)> Records)> splittedData)
        {
            List<(DateOnly Date, double PartValue)> result = new();

            foreach (var (totalValue, records) in splittedData)
            {
                var totalSeconds = records.Sum(x => x.PartSeconds);

                foreach (var record in records)
                {
                    var partValue = record.PartSeconds * totalValue / totalSeconds;
                    result.Add((record.Date, partValue));
                }
            }

            return result;
        }

        private static List<DailyEnergyConsumptionItem> ConvertToResult(List<(DateOnly Date, double PartValue)> calculatedData)
        {
            var result = calculatedData.GroupBy(x => x.Date)
                                       .ToDictionary(x => x.Key, x => x.Sum(x => x.PartValue))
                                       .Select(x => new DailyEnergyConsumptionItem() { Date = x.Key, Value = x.Value })
                                       .ToList();

            return result;
        }
    }
}
