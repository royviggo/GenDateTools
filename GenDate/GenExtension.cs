using System.Collections.Generic;

namespace GenDate
{
    public static class GenExtension
    {
        public static string ToMonthName(this int month)
        {
            return GetMonths().ContainsKey(month) ? GetMonths()[month] : "";
        }

        public static string ToSortString(this DatePart datePart)
        {
            return string.Join("", datePart.Year.ToString().PadLeft(4, '0'), datePart.Month.ToString().PadLeft(2, '0'), datePart.Day.ToString().PadLeft(2, '0'));
        }

        public static string ToIsoString(this DatePart datePart)
        {
            return string.Join("-", datePart.Year.ToString().PadLeft(4, '0'), datePart.Month.ToString().PadLeft(2, '0'), datePart.Day.ToString().PadLeft(2, '0'));
        }

        public static string ToGenString(this DatePart datePart)
        {
            var output = datePart.Day > 0 && datePart.Day <= 31 ? datePart.Day.ToString().PadLeft(2, '0') : "";
            var month = datePart.Month > 0 && datePart.Month <= 12 ? datePart.Month.ToMonthName() : "";
            var year = datePart.Year > 0 ? datePart.Year.ToString() : "";

            output += output != "" && month != "" ? " " : "";
            output += month;
            output += output != "" && year != "" ? " " : "";

            return output + year;
        }

        public static string ToGenString(this GenDate genDate)
        {
            return genDate.DateFrom.ToGenString();
        }

        private static Dictionary<int, string> GetMonths() => new Dictionary<int, string>
        {
            { 1, "Jan" },
            { 2, "Feb" },
            { 3, "Mar" },
            { 4, "Apr" },
            { 5, "May" },
            { 6, "Jun" },
            { 7, "Jul" },
            { 8, "Aug" },
            { 9, "Sep" },
            {10, "Oct" },
            {11, "Nov" },
            {12, "Dec" },
        };
    }
}