
namespace EnergyAnalyzer.Exceptions
{
    internal class ArgumentInvalidTypeException : ArgumentException
    {
        public ArgumentInvalidTypeException(string? paramName, string? message) : base(message, paramName)
        {

        }
    }
}
