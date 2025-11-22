using CommandLine;

namespace EnergyAnalyzer.Models.Options
{
    [Verb("import", HelpText = "Import enery meter entries")]
    internal class ImportOptions : IOptions
    {
        [Option('p', "path", Required = true, HelpText = "Path of import CSV file")]
        #pragma warning disable CS8618
        public string Path { get; set; }
        #pragma warning restore CS8618 
    }
}