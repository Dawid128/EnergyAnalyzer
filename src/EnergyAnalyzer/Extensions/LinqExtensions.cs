
namespace EnergyAnalyzer.Extensions
{
    internal static class LinqExtensions
    {
        internal static bool IsOrderByAscending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var sourceOrdered = source.OrderBy(keySelector);

            for (int i = 0; i < source.Count(); i++)
                if (!EqualityComparer<TSource>.Default.Equals(source.ElementAtOrDefault(i), sourceOrdered.ElementAtOrDefault(i)))
                    return false;

            return true;
        }

        internal static bool IsOrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var sourceOrdered = source.OrderByDescending(keySelector);

            for (int i = 0; i < source.Count(); i++)
                if (!EqualityComparer<TSource>.Default.Equals(source.ElementAtOrDefault(i), sourceOrdered.ElementAtOrDefault(i)))
                    return false;

            return true;
        }
    }
}
