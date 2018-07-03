using System;

namespace GenDate
{
    public class DatePart : IEquatable<DatePart>, IComparable<DatePart>
    {
        #region Properties

        public int Year { get;  set; }
        public int Month { get;  set; }
        public int Day { get;  set; }

        #endregion

        #region Constructors

        public DatePart() { }

        public DatePart(int year, int month, int day)
        {
            if (!(year >= 0 && year <= 9999 && month >= 0 && month <= 12 && day >= 0 && day <= 31))
                throw new ArgumentOutOfRangeException("Arguments out of range");

            Year = year;
            Month = month;
            Day = day;
        }

        public DatePart(long date) 
            : this((int) date / 10000, (int)(date / 100) % 100, (int) date % 100) { }

        public DatePart(string year, string month, string day)
            : this(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day)) { }

        public DatePart(string date)
            : this(GetSubString(date, 0, 4), GetSubString(date, 4, 2), GetSubString(date, 6, 2)) { }

        #endregion

        #region Methods

        public bool IsValid() => IsValid(Year, Month, Day);

        public bool IsValidDateTime() => IsValidDateTime(Year, Month, Day);

        public bool IsLeapYear() => IsLeapYear(Year);

        #endregion

        #region Overrides

        public override string ToString()
        {
            var output = Day > 0 && Day <= 31 ? Day.ToString().PadLeft(2, '0') : "";
            var month = Month > 0 && Month <= 12 ? Month.ToMonthName() : "";
            var year = Year > 0 ? Year.ToString() : "";

            output += output != "" && month != "" ? " " : "";
            output += month;
            output += output != "" && year != "" ? " " : "";

            return output + year;
        }

        public int CompareTo(DatePart other)
        {
            var sortDate = CompareValue(this);
            var sortDateOther = CompareValue(other);

            return sortDate.CompareTo(sortDateOther);
        }

        public static int CompareValue(DatePart datePart)
        {
            return datePart != null ? (datePart.Year * 10000 + datePart.Month * 100 + datePart.Day) : 0;
        }

        public static bool operator ==(DatePart datePart1, DatePart datePart2)
        {
            if (datePart1 is null)
                return false;

            return datePart1.Equals(datePart2);
        }

        public static bool operator !=(DatePart datePart1, DatePart datePart2)
        {
            if (datePart1 is null)
                return false;

            return !(datePart1 == datePart2);
        }

        public bool Equals(DatePart other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return (Year == other.Year && Month == other.Month && Day == other.Day);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (GetType() != obj.GetType())
                return false;

            return Equals((DatePart) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Year.GetHashCode();
                hashCode = (hashCode * 397) ^ Month.GetHashCode();
                hashCode = (hashCode * 397) ^ Day.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        #region Static Functions

        /// <summary>
        /// Checks if the parameters form a valid DatePart, which is a valid date except that one or 
        /// more parameters can be 0. If a parameter is 0, it means that the value is unknown.
        /// </summary>
        public static bool IsValid(int year, int month, int day)
        {
            return IsValidDate(year, month, day, 0);
        }

        /// <summary>
        /// Checks if the parameters form a valid DateTime.
        /// </summary>
        public static bool IsValidDateTime(int year, int month, int day)
        {
            return IsValidDate(year, month, day, 1);
        }

        /// <summary>
        /// Checks if a year is a leap year.
        /// </summary>
        public static bool IsLeapYear(int year)
        {
            if (year <= 0 || year > 9999)
                return false;

            return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
        }

        #endregion

        #region Private Functions

        private static string GetSubString(string source, int startIndex, int length)
        {
            var returnString = source;

            return returnString.Substring(startIndex, length);
        }

        private static bool IsValidDate(int year, int month, int day, int lowestValue)
        {
            if (year >= lowestValue && year <= 9999 && month >= lowestValue && month <= 12)
            {
                int[] days = GenExtension.IsLeapYear(year)
                    ? new[] { 31, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }
                    : new[] { 31, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

                if (day >= lowestValue && day <= days[month])
                    return true;
            }
            return false;
        }

        #endregion
    }
}