using System.ComponentModel.DataAnnotations;
using EnergyAnalyzer.Models.Attributes;

namespace EnergyAnalyzer.Models.Data
{
    [Serializable]
    internal abstract class Item
    {
        [Key]
        [ItemProperty]
        public int Id { get; set; }
    }
}
