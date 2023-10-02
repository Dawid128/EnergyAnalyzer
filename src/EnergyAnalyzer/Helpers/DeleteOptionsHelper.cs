using System.Text.RegularExpressions;

namespace EnergyAnalyzer.Helpers
{
    internal class DeleteOptionsHelper
    {
        private const string idsRangePattern = @"^([0-9]|-)+$";

        /// <summary>
        /// Take list of ID from string. 
        /// String can representing ID or range of IDs.
        /// Example Range: 5-12
        /// </summary>
        /// <param name="idsRange">ID or range of IDs as string</param>
        public IEnumerable<int> TakeIds(string idsRange)
        {
            if (!Regex.IsMatch(idsRange, idsRangePattern))
                throw new Exception("Ids invalid. Allowed are numbers and one separator '-' to define range.");

            var split = idsRange.Split("-");
            if (split.Length > 2)
                throw new Exception("Ids invalid. Separator '-' to define range can be define one time");

            var minId = int.Parse(split[0]);
            if (split.Length == 1)
                yield return minId;

            var maxId = int.Parse(split[1]);
            if (minId >= maxId)
                throw new Exception("Ids invalid. Defined range is wrong.");

            for (int i = minId; i <= maxId; i++) 
                yield return i;
        }
    }
}
