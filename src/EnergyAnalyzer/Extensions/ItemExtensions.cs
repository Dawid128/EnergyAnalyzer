using EnergyAnalyzer.Models.Data;
using System.Reflection;

namespace EnergyAnalyzer.Extensions
{
    internal static class ItemExtensions
    {
        public static IEnumerable<(PropertyInfo PropertyInfo, T2 Attribute)> GetProperties<T1, T2>() where T1 : Item where T2 : Attribute
        {
            var properties = typeof(T1).GetProperties();
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<T2>();
                if (attribute is null)
                    continue;

                yield return (property, attribute);
            }
        }
    }
}
