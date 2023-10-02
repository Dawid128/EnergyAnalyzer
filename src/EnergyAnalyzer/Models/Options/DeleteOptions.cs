using CommandLine;

namespace EnergyAnalyzer.Models.Options
{
    [Verb("delete", HelpText = "Delety energy meter entry")]
    internal class DeleteOptions : IOptions
    {
        [Option('i', "id", Required = false, Separator = ';', HelpText = "Id/s of Energy Meter Entry to delete.\r\nUse separator ';' to define more Ids.\r\nUse separator '-' to define range.\r\nExample: 1-5;10-15")]
        public IEnumerable<string> Ids { get; set; }

        [Option('a', "all", Required = false, HelpText = "Use to delete all Enery Meter Entries")]
        public bool All { get; set; }
    }
}
