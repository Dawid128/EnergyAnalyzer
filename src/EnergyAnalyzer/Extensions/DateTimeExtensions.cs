
namespace EnergyAnalyzer.Extensions
{
    internal static class DateTimeExtensions
    {
        public static bool IsSameDay(this DateTime value, DateTime compareDateTime)
        {
            if (value.Date == compareDateTime.Date)
                return true;

            return false;
        }

        public static DateOnly ToDateOnly(this DateTime value) => new(value.Year, value.Month, value.Day);

        public static DateTime ToDateTime(this DateOnly value) => new(value.Year, value.Month, value.Day);
    }
}
