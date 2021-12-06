using System.Collections.Generic;

namespace GenDateTools
{
    public static class GenTools
    {
        private static Dictionary<int, string> MonthsFromNumber()
        {
            return new Dictionary<int, string>
            {
                { 1, "Jan" }, { 2, "Feb" }, { 3, "Mar" }, { 4, "Apr" }, { 5, "May" }, { 6, "Jun" },
                { 7, "Jul" }, { 8, "Aug" }, { 9, "Sep" }, {10, "Oct" }, {11, "Nov" }, {12, "Dec" },
            };
        }

        public static Dictionary<string, int> MonthsFromName()
        {
            return MonthsFromNumber().ReverseUpper();
        }

        /// <summary>
        /// Takes a month number as parameter and returns a three character long month name.
        /// </summary>
        /// <param name="month">Month number from 1 to 12</param>
        public static string MonthName(int month)
        {
            return MonthsFromNumber().ContainsKey(month) ? MonthsFromNumber()[month] : "";
        }

        private static Dictionary<string, int> ReverseUpper(this IDictionary<int, string> source)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (KeyValuePair<int, string> entry in source)
            {
                string entryUpper = entry.Value.ToUpperInvariant();

                if (!dictionary.ContainsKey(entryUpper))
                {
                    dictionary.Add(entryUpper, entry.Key);
                }
            }
            return dictionary;
        }
    }
}
