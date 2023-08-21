using EnergyAnalyzer.DataManagers.Interfaces;
using EnergyAnalyzer.Exceptions;
using EnergyAnalyzer.Models.Data;
using System.IO;
using System.Text;

namespace EnergyAnalyzer.DataManagers
{
    internal class FileCSVDataManager : IDataManager
    {
        private const string _energyMeterEntriesFileName = "EnergyMeterEntries.txt";

        private readonly string _path;

        public FileCSVDataManager(string path)
        {
            _path = path;

            Initialize(path);
        }

        public async Task Write(EnergyMeterEntryItem item)
        {
            var line = ConvertToString(item);

            await WriteNextLine(line);
        }

        public async Task Delete(EnergyMeterEntryItem item)
        {
            var line = ConvertToString(item);

            await DeleteLine(line);
        }

        public async Task<EnergyMeterEntryItem?> ReadLastOrDefaultAsync()
        {
            var lastLine = await ReadLastLine();
            if (lastLine is null)
                return null;

            return ConvertFromString(lastLine);
        }

        public async Task<EnergyMeterEntryItem?> FindAsync(int id)
        {
            var contentLine = ConvertIdToString(id);

            var line = await ReadFirstOrDefaultLine(contentLine);
            if (line is null)
                return null;

            return ConvertFromString(line);
        }

        public async Task<List<EnergyMeterEntryItem>> ReadAllAsync()
        {
            var lines = await ReadAllLines();

            return lines.Select(ConvertFromString).ToList();
        }

        #region Initialize

        private static void Initialize(string path)
        {
            if (!File.Exists(Path.Combine(path, _energyMeterEntriesFileName)))
            {
                using var fs = File.Create(Path.Combine(path, _energyMeterEntriesFileName));
                fs.Dispose();
            }
        }

        #endregion Initialize

        #region From File

        private async Task<string?> ReadLastLine()
        {
            using var sr = new StreamReader(Path.Combine(_path, _energyMeterEntriesFileName), Encoding.UTF8);

            string? lastLine = null;

            string? line = null;
            while ((line = await sr.ReadLineAsync()) is not null)
                lastLine = line;

            return lastLine;
        }

        private async Task<string?> ReadFirstOrDefaultLine(string contentLine)
        {
            using var sr = new StreamReader(Path.Combine(_path, _energyMeterEntriesFileName), Encoding.UTF8);

            string? line;
            while ((line = await sr.ReadLineAsync()) is not null)
                if (line.StartsWith(contentLine))
                    return line;

            return null;
        }

        private async Task<List<string>> ReadAllLines()
        {
            using var sr = new StreamReader(Path.Combine(_path, _energyMeterEntriesFileName), Encoding.UTF8);

            var lines = new List<string>();

            string? line = null;
            while ((line = await sr.ReadLineAsync()) is not null)
                lines.Add(line);

            return lines;
        }

        private async Task WriteNextLine(string line)
        {
            using var sw = new StreamWriter(Path.Combine(_path, _energyMeterEntriesFileName), true, Encoding.UTF8);

            await sw.WriteLineAsync(line);
        }

        private async Task<bool> DeleteLine(string line)
        {
            var tempFileName = $"{Guid.NewGuid()}_{_energyMeterEntriesFileName}";

            //Duplicate file
            File.Copy(Path.Combine(_path, _energyMeterEntriesFileName), Path.Combine(_path, tempFileName));

            try
            {
                //Clear initialize file
                await File.WriteAllTextAsync(Path.Combine(_path, _energyMeterEntriesFileName), string.Empty);

                //Move entries from temp to initialize file (except line to delete)
                using var sr = new StreamReader(Path.Combine(_path, tempFileName), Encoding.UTF8);
                using var sw = new StreamWriter(Path.Combine(_path, _energyMeterEntriesFileName), true, Encoding.UTF8);

                bool success = false;
                string? tempLine = null;
                while ((tempLine = await sr.ReadLineAsync()) is not null)
                {
                    if (tempLine == line)
                    {
                        success = true;
                        continue;
                    }

                    await sw.WriteLineAsync(tempLine);
                }

                return success;
            }
            finally
            {
                //Delete temp file
                File.Delete(Path.Combine(_path, tempFileName));
            }
        }

        #endregion From File

        #region Converters

        private static EnergyMeterEntryItem ConvertFromString(string line)
        {
            var split = line.Split(";");
            if (split.Length is not 3)
                throw new ArgumentOutOfRangeException(nameof(split), $"TODO");

            if (!int.TryParse(split[0], out var id))
                throw new ArgumentInvalidTypeException(nameof(id), $"TODO");

            if (!DateTime.TryParse(split[1], out var date))
                throw new ArgumentInvalidTypeException(nameof(date), $"TODO");

            if (!int.TryParse(split[2], out var value))
                throw new ArgumentInvalidTypeException(nameof(value), $"TODO");

            return new EnergyMeterEntryItem()
            {
                Id = id,
                Date = date,
                Value = value
            };
        }

        private static string ConvertToString(EnergyMeterEntryItem item)
        {
            return $"{item.Id};{item.Date:dd.MM.yyyy HH:mm:ss};{item.Value}";
        }

        private static string ConvertIdToString(int id)
        {
            return $"{id};";
        }

        #endregion Converters
    }
}
