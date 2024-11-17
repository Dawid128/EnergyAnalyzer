
namespace EnergyAnalyzer.NET
{
    internal sealed class ServiceProviderWrapper(IServiceProvider serviceProvider) : IServiceProviderWrapper
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public object? GetRequiredService(Type serviceType) => _serviceProvider.GetRequiredService(serviceType);
    }
}
