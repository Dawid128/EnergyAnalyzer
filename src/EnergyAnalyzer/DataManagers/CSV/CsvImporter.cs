using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Extensions;
using EnergyAnalyzer.Models.Attributes;
using EnergyAnalyzer.Models.Data;
using EnergyAnalyzer.Models.DataManagers.CSV;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;

namespace EnergyAnalyzer.DataManagers.CSV
{
    /// <summary>
    /// CsvImporter allows importing data from CSV files into objects of type <see cref="Item"/>.
    /// </summary>
    /// <remarks>
    /// This class uses <see cref="CsvImporterOptions"/> to configure the CSV import.<br/>
    /// The order of imported properties is determined by the <see cref="ItemImportAttribute.ImportOrder"/> attribute.
    /// </remarks>
    internal class CsvImporter(IOptions<CsvImporterOptions> options) : IImporter
    {
        private readonly CsvImporterOptions _options = options.Value ?? new CsvImporterOptions();

        /// <summary>
        /// Imports data from a CSV file and maps it to objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// Each line of the CSV file is split according to the separator specified in <see cref="CsvImporterOptions.Separator"/>.<br/>
        /// Values are converted to the target property types, such as <see cref="DateTime"/>, <see cref="int"/>, <see cref="double"/>, or <see cref="string"/>.<br/>
        /// Lines containing fewer values than the number of mapped properties are skipped.
        /// </remarks>
        /// <param name="filePath">The path to the CSV file to import.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>A collection of objects of type <typeparamref name="T"/> populated with data from the CSV file.</returns>
        /// <exception cref="Exception">Thrown when no properties with <see cref="ItemImportAttribute"/> are found for import.</exception>
        public async Task<IEnumerable<T>> ImportAsync<T>(string filePath, CancellationToken cancellationToken = default) where T : Item, new()
        {
            var properties = GetProperties<T>().ToList();
            if (properties.Count == 0)
            {
                throw new Exception("No properties found for import");
            }

            var result = new List<T>();
            using var reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var line = await reader.ReadLineAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(_options.Separator);
                if (parts.Length < properties.Count)
                    continue;

                var item = new T();
                for (int i = 0; i < properties.Count; i++)
                {
                    var prop = properties[i];
                    var rawValue = parts[i];

                    object? convertedValue = ConvertValue(rawValue, prop.PropertyType);

                    prop.SetValue(item, convertedValue);
                }

                result.Add(item);
            }

            return result;
        }

        private IEnumerable<PropertyInfo> GetProperties<T>() where T : Item, new()
        {
            var properties = ItemExtensions.GetProperties<T, ItemImportAttribute>();
            properties = properties.OrderBy(x=>x.Attribute.ImportOrder);
            foreach (var (propInfo, attr) in properties)
            {
                yield return propInfo;
            }
        }

        private object? ConvertValue(string raw, Type targetType)
        {
            if (targetType == typeof(DateTime))
                return DateTime.Parse(raw, _options.ParsingCulture);

            if (targetType == typeof(int))
                return int.Parse(raw);

            if (targetType == typeof(double))
                return double.Parse(raw, CultureInfo.InvariantCulture);

            if (targetType == typeof(string))
                return raw;

            return Convert.ChangeType(raw, targetType, CultureInfo.InvariantCulture);
        }
    }
}
