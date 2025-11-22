
namespace EnergyAnalyzer.Models.Attributes
{
    /// <summary>
    /// Specifies that a property of an <see cref="Item"/> can be imported and defines its order in the import process.
    /// </summary>
    /// <remarks>
    /// The <see cref="ImportOrder"/> property determines the sequence in which properties are read from the source file.<br/>
    /// Lower numbers are imported first. Default value is 0.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    internal class ItemImportAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the order in which the property should be imported.
        /// </summary>
        public int ImportOrder { get; set; } = 0;
    }
}
