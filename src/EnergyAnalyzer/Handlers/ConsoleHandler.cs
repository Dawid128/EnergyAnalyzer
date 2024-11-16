using EnergyAnalyzer.Extensions;
using EnergyAnalyzer.Models.Data;
using Spectre.Console;

namespace EnergyAnalyzer.Handlers
{
    internal class ConsoleHandler
    {
        public void WriteObjects<T>(IList<T> items, IList<(string Name, string? Format)> columns) where T : Item
        {
            var table = new Table();

            var properties = ItemExtensions.GetProperties<T>();

            foreach (var (columnName, _) in columns)
                table.AddColumn(columnName);

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var color = "[white]";

                table.AddEmptyRow();
                for (int j = 0; j < columns.Count; j++)
                {
                    var (columnName, columnFormat) = columns[j];

                    if (columnName.Equals("Number", StringComparison.OrdinalIgnoreCase))
                    {
                        table.UpdateCell(i, j, $"{color}{i + 1}[/]");
                        continue;
                    }

                    var property = properties.FirstOrDefault(x => x.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
                    if (property is null)
                        throw new Exception($"Not found column {columnName} for type {typeof(T).Name}");

                    var value = property.GetValue(item);

                    string? valueStr = null;
                    valueStr = value is IFormattable formattable ? formattable.ToString(columnFormat, null) : value?.ToString();
                    if (valueStr is null)
                        continue;

                    table.UpdateCell(i, j, $"{color}{valueStr}[/]");
                }
            }

            AnsiConsole.Write(table);
        }
    }
}
