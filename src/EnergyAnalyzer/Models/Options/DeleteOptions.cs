using CommandLine;

namespace EnergyAnalyzer.Models.Options
{
    [Verb("delete", HelpText = "Delety energy meter entry")]
    internal class DeleteOptions : IOptions
    {
        [Option("id", Required = true, HelpText = "Id of Energy Meter Entry to delete")]
        public int Id { get; set; }
    }
}
