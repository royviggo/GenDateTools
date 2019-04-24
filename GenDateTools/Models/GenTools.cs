using System.Collections.Generic;

namespace GenDateTools
{
    public static class GenTools
    {
        public static Dictionary<int, string> MonthsFromNumber()
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

        /// <summary>
        /// Returns substring instead of modifying the string.
        /// </summary>
        /// <param name="source">String to find substring in</param>
        /// <param name="startIndex">Start position</param>
        /// <param name="length">Length of the string</param>
        /// <returns></returns>
        public static string GetSubString(string source, int startIndex, int length)
        {
            var returnString = source;

            return returnString.Substring(startIndex, length);
        }

        private static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            var dictionary = new Dictionary<TValue, TKey>();
            foreach (var entry in source)
            {
                if (!dictionary.ContainsKey(entry.Value))
                    dictionary.Add(entry.Value, entry.Key);
            }
            return dictionary;
        }

        private static Dictionary<string, int> ReverseUpper(this IDictionary<int, string> source)
        {
            var dictionary = new Dictionary<string, int>();
            foreach (var entry in source)
            {
                var entryUpper = entry.Value.ToString().ToUpper();

                if (!dictionary.ContainsKey(entryUpper))
                    dictionary.Add(entryUpper, entry.Key);
            }
            return dictionary;
        }
    }
}
