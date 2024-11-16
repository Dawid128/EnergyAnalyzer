using CommandLine;

namespace EnergyAnalyzer.Models.Options
{
    [Verb("createAD", HelpText = "Create daily analysis of energy consumptions.")]
    internal class CreateAnalysisOptions : IOptions
    {
        [Option('s', "startDate", Required = true, HelpText = "Set start date of period for the analysis.")]
        public DateTime? StartDate { get; set; }

        [Option('e', "endDate", Required = true, HelpText = "Set end date of period for the analysis.")]
        public DateTime? EndDate { get; set; } 
    }
}