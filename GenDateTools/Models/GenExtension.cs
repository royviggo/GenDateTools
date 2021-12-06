using System.Globalization;

namespace GenDateTools
{
    public static class GenExtension
    {
        public static string ToMonthName(this int month)
        {
            return GenTools.MonthName(month);
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
            return datePart.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToGenString(this GenDate genDate)
        {
            return genDate.ToString();
        }

        public static bool IsLeapYear(this int year)
        {
            return DatePart.IsLeapYear(year);
        }
    }
}