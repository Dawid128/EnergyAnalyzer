using EnergyAnalyzer.Models.Data;

namespace EnergyAnalyzer.DataManagers.Interfaces
{
    /// <summary>
    /// Defines a contract for importing data from an external source into a collection of <see cref="Item"/> objects.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface are responsible for reading and converting data from a specific source,
    /// such as CSV files, into objects of type <typeparamref name="T"/>.
    /// </remarks>
    internal interface IImporter
    {
        /// <summary>
        /// Defines a contract for importing data from a source into a collection of <see cref="Item"/> objects.
        /// </summary>
        /// <remarks>
        /// Implementations should provide logic to read and convert data from a specific source, such as a CSV file,
        /// into objects of type <typeparamref name="T"/>.
        /// </remarks>
        /// <typeparam name="T">The type of item to import. Must inherit from <see cref="Item"/> and have a parameterless constructor.</typeparam>
        /// <param name="filePath">The path to the source file to import.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation. The result is a collection of imported <typeparamref name="T"/> objects.</returns>
        Task<IEnumerable<T>> ImportAsync<T>(string filePath, CancellationToken cancellationToken = default) where T : Item, new();
    }
}
