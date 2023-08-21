//using EnergyAnalyzer.Exceptions;
//using EnergyAnalyzer.Models.Options;
//using EnergyAnalyzer.OptionsManagers.Interfaces;

//namespace EnergyAnalyzer.OptionsManagers.Managers
//{
//    internal class SomeOptionsManager : IOptionsManager
//    {
//        public SomeOptionsManager()
//        {
//        }

//        public async Task ExecuteAsync(IOptions options)
//        {
//            if (options is not SomeOptions someOptions)
//                throw new ArgumentInvalidTypeException(nameof(options), $"Argument options is not valid type {nameof(SomeOptions)}");
//        }

//        public Type GetOptionsType() => typeof(SomeOptions);
//    }
//}
