using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.NET;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Service
{
    internal class OptionsManagerService
    {
        private readonly IServiceProviderWrapper _serviceProviderWrapper;
        private readonly Dictionary<Type, Type> _mapOptionsManagerTypes;

        public OptionsManagerService(IServiceProviderWrapper serviceProviderWrapper) 
        {
            _serviceProviderWrapper = serviceProviderWrapper;

            _mapOptionsManagerTypes = GetMapOptionsManagerTypes();
        }

        public List<Type> GetAllTypesIOptions() => _mapOptionsManagerTypes.Select(x => x.Key).ToList();

        public IOptionsManager CreateOptionsManager(IOptions options)
        {
            if (!_mapOptionsManagerTypes.TryGetValue(options.GetType(), out var optionsManagerType))
                throw new KeyNotFoundException($"Not found mapped IOptionsManager, for type {options.GetType().Name}");

            var optionsManager = _serviceProviderWrapper.GetRequiredService(optionsManagerType) as IOptionsManager;
            if (optionsManager is null)
                throw new InvalidCastException($"Get Service of type {optionsManagerType.Name} failed");

            return optionsManager;
        }
        
        private Dictionary<Type, Type> GetMapOptionsManagerTypes()
        {
            var result = new Dictionary<Type, Type>();

            var allOptionsManagerTypes = GetAllInterfaceTypes<IOptionsManager>("EnergyAnalyzer.OptionsManagers.Managers");
            var allOptionsTypes = GetAllInterfaceTypes<IOptions>("EnergyAnalyzer.Models.Options");

            foreach (var optionsType in allOptionsTypes)
            {
                var optionsManagerTypeName = $"{optionsType.Name}Manager";

                var optionsManagerType = allOptionsManagerTypes.Single(x=>x.Name == optionsManagerTypeName);
                result.Add(optionsType, optionsManagerType);
            }

            return result;
        }

        private List<Type> GetAllInterfaceTypes<T>(string @namespace)
        {
            var iType = typeof(T);

            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                  .SelectMany(s => s.GetTypes())
                                                  .Where(x => x.Namespace?.Equals(@namespace) is true)
                                                  .Where(w => iType.IsAssignableFrom(w) && w.IsClass)
                                                  .ToList();

            return allTypes;
        }
    }
}
