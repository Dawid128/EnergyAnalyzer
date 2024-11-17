using EnergyAnalyzer.OptionsManagers.Service;
using EnergyAnalyzer.Models.Options;
using EnergyAnalyzer.NET;
using Moq;
using EnergyAnalyzer.OptionsManagers.Interfaces;

namespace EnergyAnalyzer.Test.OptionsManagers.Service
{
    public class OptionsManagerServiceTests
    {
        private readonly Mock<IServiceProviderWrapper> _mockServiceProviderWrapper;
        private readonly Mock<IOptionsManager> _mockOptionsManager;

        public OptionsManagerServiceTests()
        {
            _mockServiceProviderWrapper = new Mock<IServiceProviderWrapper>();
            _mockOptionsManager = new Mock<IOptionsManager>();

            _mockServiceProviderWrapper.Setup(sp => sp.GetRequiredService(It.Is<Type>(x => typeof(IOptionsManager).IsAssignableFrom(x))))
                                       .Returns((Type type) => _mockOptionsManager.Object);
        }

        [Theory]
        [InlineData(typeof(AddOptions))]
        [InlineData(typeof(CreateAnalysisOptions))]
        [InlineData(typeof(DeleteOptions))]
        [InlineData(typeof(ShowOptions))]
        public void CreateOptionsManager_ShouldReturnOptionsManager_ForValidType(Type optionsType)
        {
            //Mock
            var optionsManagerService = new OptionsManagerService(_mockServiceProviderWrapper.Object);
            var optionsInstance = Activator.CreateInstance(optionsType) as IOptions;
            if (optionsInstance is null)
                Assert.Fail($"Create Instance of type {optionsType.Name} failed");

            //Test
            var result = optionsManagerService.CreateOptionsManager(optionsInstance);

            //Result
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateOptionsManager_ShouldReturnKeyNotFoundException_ForInvalidType()
        {
            //Create Mock and Data
            var mockOptions = new Mock<IOptions>();
            var optionsManagerService = new OptionsManagerService(_mockServiceProviderWrapper.Object);

            //Test and Result
            Assert.ThrowsAny<KeyNotFoundException>(() => optionsManagerService.CreateOptionsManager(mockOptions.Object));
        }

        [Fact]
        public void CreateOptionsManager_ShouldReturnInvalidCastException_ForInvalidType()
        {
            //Mock
            _mockServiceProviderWrapper.Setup(sp => sp.GetRequiredService(It.Is<Type>(x => typeof(IOptionsManager).IsAssignableFrom(x))))
                                       .Returns((Type type) => null);

            var optionsManagerService = new OptionsManagerService(_mockServiceProviderWrapper.Object);
            var optionsInstance = Activator.CreateInstance(typeof(AddOptions)) as IOptions;
            if (optionsInstance is null)
                Assert.Fail($"Create Instance of type {typeof(AddOptions).Name} failed");

            //Test and Result
            Assert.ThrowsAny<InvalidCastException>(() => optionsManagerService.CreateOptionsManager(optionsInstance));
        }

        [Fact]
        public void GetAllTypesIOptions_ShouldReturnAllOptions_Always()
        {
            //Mock
            var optionsManagerService = new OptionsManagerService(_mockServiceProviderWrapper.Object);

            //Execute
            var allTypes = optionsManagerService.GetAllTypesIOptions();

            var requiredTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                       .Where(x => x.ManifestModule?.Name?.Equals("EnergyAnalyzer.dll") is true)
                                                       .SelectMany(s => s.GetTypes())
                                                       .Where(w => typeof(IOptions).IsAssignableFrom(w) && w.IsClass)
                                                       .ToList();

            //Test and Result
            Assert.Equal(allTypes.Count, requiredTypes.Count);
            foreach (var requiredType in requiredTypes)
                Assert.Equal(requiredType, allTypes.FirstOrDefault(x => x == requiredType));
        }
    }
}
