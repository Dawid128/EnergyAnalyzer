using EnergyAnalyzer.Models.Attributes;
using EnergyAnalyzer.Models.Data;
using System.Reflection;

namespace EnergyAnalyzer.Extensions
{
    internal static class ItemExtensions
    {
        public static List<PropertyInfo> GetProperties<T>() where T : Item
        {
            var resultRaw = new List<PropertyInfo>();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<ItemPropertyAttribute>();
                if (attribute is null)
                    continue;

                resultRaw.Add(property);
            }

            return resultRaw.ToList();
        }
    }
}
