using EnergyAnalyzer.DataManagers.CSV;
using System.Globalization;

namespace EnergyAnalyzer.Models.DataManagers.CSV
{
    /// <summary>
    /// Options for configuring the behavior of <see cref="CsvImporter"/>.
    /// </summary>
    /// <remarks>
    /// Allows specifying the CSV separator character and the culture used for parsing values.
    /// </remarks>
    internal sealed class CsvImporterOptions
    {
        /// <summary>
        /// Gets or sets the character used to separate values in the CSV file.<br/>
        /// Default is ';'.
        /// </summary>
        public char Separator { get; set; } = ';';

        /// <summary>
        /// Gets or sets the <see cref="CultureInfo"/> used when parsing values from the CSV file.<br/>
        /// Default is <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        public CultureInfo ParsingCulture { get; set; } = CultureInfo.InvariantCulture;
    }
}
