using CommandLine;

namespace EnergyAnalyzer.Models.Options
{
    [Verb("add", HelpText = "Add energy meter entry")]
    internal class AddOptions : IOptions
    {
        [Option('v', "value", Required = true, HelpText = "Date of read data energy meter")]
        public int Value { get; set; }

        [Option('d', "date", HelpText = "Date of read data energy meter. Default is today")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}