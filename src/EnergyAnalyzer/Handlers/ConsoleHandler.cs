using EnergyAnalyzer.Extensions;
using EnergyAnalyzer.Models.Data;
using Spectre.Console;

namespace EnergyAnalyzer.Handlers
{
    internal class ConsoleHandler
    {
        public void WriteObjects<T>(IList<T> items, IList<string> columns) where T : Item
        {
            var table = new Table();

            var properties = ItemExtensions.GetProperties<T>();

            foreach (var column in columns)
                table.AddColumn(column);

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var color = "[white]";

                table.AddEmptyRow();
                for (int j = 0; j < columns.Count; j++)
                {
                    var column = columns[j];

                    if (column.Equals("Number", StringComparison.OrdinalIgnoreCase))
                    {
                        table.UpdateCell(i, j, $"{color}{i + 1}[/]");
                        continue;
                    }

                    var property = properties.FirstOrDefault(x => x.Name.Equals(column, StringComparison.OrdinalIgnoreCase));
                    if (property is null)
                        throw new Exception($"Not found column {column} for type {typeof(T).Name}");

                    var value = property.GetValue(item);

                    string? valueStr = null;
                    valueStr = value?.ToString();
                    if (valueStr is null)
                        continue;

                    table.UpdateCell(i, j, $"{color}{valueStr}[/]");
                }
            }

            AnsiConsole.Write(table);
        }
    }
}
