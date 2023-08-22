using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.OptionsManagers.Service
{
    internal class OptionsManagerService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, Type> _mapOptionsManagerTypes;

        public OptionsManagerService(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider; ;

            _mapOptionsManagerTypes = GetMapOptionsManagerTypes();
        }

        public List<Type> GetAllTypesIOptions() => _mapOptionsManagerTypes.Select(x => x.Key).ToList();

        public IOptionsManager CreateOptionsManager(IOptions options)
        {
            if (!_mapOptionsManagerTypes.TryGetValue(options.GetType(), out var optionsManagerType))
                throw new KeyNotFoundException($"Not found mapped IOptionsManager, for type {options.GetType().Name}");

            var optionsManager = _serviceProvider.GetRequiredService(optionsManagerType) as IOptionsManager;
            if (optionsManager is null)
                throw new InvalidCastException($"Get Service of type {optionsManagerType.Name} failed");

            return optionsManager;
        }
        
        private Dictionary<Type, Type> GetMapOptionsManagerTypes()
        {
            var result = new Dictionary<Type, Type>();

            var allOptionsManagerTypes = GetAllInterfaceTypes<IOptionsManager>();
            var allOptionsTypes = GetAllInterfaceTypes<IOptions>();

            if (allOptionsManagerTypes.Count != allOptionsTypes.Count)
                throw new ArgumentOutOfRangeException($"Count of types {nameof(IOptionsManager)} and {nameof(IOptions)} cannot be different");

            foreach (var optionsType in allOptionsTypes)
            {
                var optionsManagerTypeName = $"{optionsType.Name}Manager";

                var optionsManagerType = allOptionsManagerTypes.Single(x=>x.Name == optionsManagerTypeName);
                result.Add(optionsType, optionsManagerType);
            }

            return result;
        }

        private List<Type> GetAllInterfaceTypes<T>()
        {
            var iType = typeof(T);

            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                  .SelectMany(s => s.GetTypes())
                                                  .Where(w => iType.IsAssignableFrom(w) && w.IsClass)
                                                  .ToList();

            return allTypes;
        }
    }
}
