using System;

namespace GenDate
{
    public class DatePart : IEquatable<DatePart>, IComparable<DatePart>
    {
        public int Year { get;  set; }
        public int Month { get;  set; }
        public int Day { get;  set; }

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

        private static string GetSubString(string source, int startIndex, int length)
        {
            var returnString = source;

            return returnString.Substring(startIndex, length);
        }
    }
}