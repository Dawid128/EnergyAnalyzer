using EnergyAnalyzer.Database;
using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Extensions;
using EnergyAnalyzer.Handlers;
using EnergyAnalyzer.Models.Data;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.OptionsManagers.Interfaces;
using EnergyAnalyzer.Services;

namespace EnergyAnalyzer.OptionsManagers.Managers
{
    internal class CreateAnalysisOptionsManager : IOptionsManager
    {
        private readonly DailyEnergyConsumptions _dailyEnergyConsumptions;
        private readonly IReader _reader;
        private readonly DatabaseContext _context;
        private readonly ConsoleHandler _consoleHandler;

        public CreateAnalysisOptionsManager(DailyEnergyConsumptions dailyEnergyConsumptions, IReader reader, DatabaseContext context, ConsoleHandler consoleHandler)
        {
            _dailyEnergyConsumptions = dailyEnergyConsumptions;
            _reader = reader;
            _context = context;
            _consoleHandler = consoleHandler;
        }

        public async Task ExecuteAsync(IOptions options)
        {
            if (options is not CreateAnalysisOptions createAnalysisOptions)
                throw new ArgumentInvalidTypeException(nameof(options), $"Argument options is not valid type {nameof(CreateAnalysisOptions)}");

            if (createAnalysisOptions.StartDate > createAnalysisOptions.EndDate)
                throw new ArgumentOutOfRangeException(nameof(createAnalysisOptions), "End date can not be less than start date");

            var startDate = createAnalysisOptions.StartDate ?? throw new ArgumentNullException(nameof(createAnalysisOptions.StartDate));
            startDate = startDate.ToDateOnly().ToDateTime();

            var endDate = createAnalysisOptions.EndDate ?? throw new ArgumentNullException(nameof(createAnalysisOptions.EndDate));
            endDate = endDate.ToDateOnly().AddDays(1).ToDateTime();

            var energyMeterItems = await _reader.ReadAllAsync<EnergyMeterEntryItem>(_context, x => x.Date >= startDate && x.Date < endDate);
            var dailyEnergyItems = _dailyEnergyConsumptions.Calculate(energyMeterItems);

            _consoleHandler.WriteObjects(dailyEnergyItems, new[] { ("Date", null), ("Value", "0.00") });
        }

        public Type GetOptionsType() => typeof(CreateAnalysisOptions);
    }
}
