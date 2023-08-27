
namespace EnergyAnalyzer.Helpers
{
    internal class ReflectionHelper
    {
        public Dictionary<string, object> GetMetadata(string nameVar, object valueVar)
        {
            var result = new Dictionary<string, object>();

            var typeVar = valueVar.GetType();
            if (typeVar.IsPrimitive || typeVar == typeof(decimal) || typeVar == typeof(string) || typeVar == typeof(DateTime))
            {
                result.Add(nameVar, valueVar);
                return result;
            }

            var properties = typeVar.GetProperties();
            foreach (var property in properties)
            {
                var nameVarSub = $"{nameVar}.{property.Name}";
                var valueVarSub = property.GetValue(valueVar);
                if (valueVarSub is null)
                    continue;

                foreach (var item in GetMetadata(nameVarSub, valueVarSub))
                    result.Add(item.Key, item.Value);
            }

            return result;
        }
    }
}
