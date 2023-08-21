using CommandLine;

namespace EnergyAnalyzer.Models.Options
{
    [Verb("show", HelpText = "Show energy meter entries.")]
    internal class ShowOptions : IOptions
    {
        [Option('m', "mode", HelpText = "Mode of filter. [equal, between, less, greater]. Default is equal.")]
        public ShowOptionsFilterMode FilterMode { get; set; }

        [Option('p', "property", HelpText = "Name of property to filter. Default is Id.")]
        public string PropertyName { get; set; } = "Id";

        [Option('v', "value", HelpText = "Value to filter. Required if mode has value: equal, less, greater.")]
        public object? Value { get; set; }

        [Option('f', "from", HelpText = "Value start range to filter. Required if mode has value: between.")]
        public object? From { get; set; }

        [Option('t', "to", HelpText = "Value end range to filter. Required if mode has value: between.")]
        public object? To { get; set; }
    }

    internal enum ShowOptionsFilterMode
    {
        Equal,
        Between, 
        Less,
        Greater
    }
}
