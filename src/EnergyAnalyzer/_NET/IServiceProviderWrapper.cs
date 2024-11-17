
namespace EnergyAnalyzer.NET
{
    internal interface IServiceProviderWrapper
    {
        public object? GetRequiredService(Type serviceType);
    }
}
