namespace GenDate
{
    public static class GenExtension
    {
        public static string ToMonthName(this int month)
        {
            return DatePart.MonthName(month);
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
            return datePart.ToString();
        }

        public static string ToGenString(this GenDate genDate)
        {
            return genDate.DateFrom.ToString();
        }

        public static bool IsLeapYear(this int year)
        {
            return DatePart.IsLeapYear(year);
        }

        public static bool IsValidDate(this DatePart datePart)
        {
            return DatePart.IsValidDate(datePart.Year, datePart.Month, datePart.Day);
        }

        public static bool IsValidDateTime(this DatePart datePart)
        {
            return DatePart.IsValidDateTime(datePart.Year, datePart.Month, datePart.Day);
        }
    }
}