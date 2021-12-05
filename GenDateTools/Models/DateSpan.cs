using System;

namespace GenDateTools
{
    public class DateSpan : IEquatable<DateSpan>, IComparable<DateSpan>
    {
        private const int _daysPerYear = 365;

        internal int _years;
        internal int _days;
        internal int _totalDays;

        public DateSpan(int totalDays)
        {
            _totalDays = totalDays;
            _years = totalDays / _daysPerYear;
            _days = totalDays % _daysPerYear;
        }

        public DateSpan(int years, int days)
        {
            _years = years;
            _days = days;
            _totalDays = (years * _daysPerYear) + days;
        }

        public DateSpan(DatePart datePart1, DatePart datePart2)
        {
            if (datePart1 > datePart2)
            {
                var datePartTemp = datePart1;
                datePart1 = datePart2;
                datePart2 = datePartTemp;
            }

            _years = 0;
            _totalDays = DatePart.DaysInYear(datePart1.Year) - datePart1.DayOfYear() - (DatePart.DaysInYear(datePart2.Year) - datePart2.DayOfYear());
            _days = datePart2.DayOfYear() - datePart1.DayOfYear();

            var processYear = datePart1.Year;

            while (processYear < datePart2.Year)
            {
                processYear++;
                _totalDays += DatePart.DaysInYear(processYear);
                _years++;
            }

            if (_days < 0)
            {
                _years = _years > 0 ? _years - 1 : _years;
                _days += DatePart.DaysInYear(datePart2.Year - 1) + (_years > 0 ? 1 : 0);
            }

            if (_days == DatePart.DaysInYear(datePart2.Year))
            {
                _days = 0;
                _years++;
            }
        }

        public int Days => _days;
        public int Years => _years;
        public int TotalDays => _totalDays;

        public static int Compare(DateSpan dateSpan1, DateSpan dateSpan2)
        {
            if (dateSpan1.TotalDays > dateSpan2.TotalDays)
            {
                return 1;
            }

            if (dateSpan1.TotalDays < dateSpan2.TotalDays)
            {
                return -1;
            }

            return 0;
        }

        public int CompareTo(DateSpan other)
        {
            return Compare(this, other);
        }

        public bool Equals(DateSpan other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (TotalDays == other.TotalDays);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((DateSpan)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TotalDays.GetHashCode();
                hashCode = (hashCode * 397) ^ Years.GetHashCode();
                hashCode = (hashCode * 397) ^ Days.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            var years = $"{Years} year{(Years > 1 ? "s" : "")}";
            var days = $"{Days} day{(Days == 0 || Days > 1 ? "s" : "")}";

            var output = Years > 0 ? years : "";
            output += (Years > 0 && Days > 0) ? " and " : "";

            return output + ((Days > 0 || Years == 0) ? days : "");
        }

        public static bool operator ==(DateSpan left, DateSpan right)
        {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(DateSpan left, DateSpan right)
        {
            return left.CompareTo(right) != 0;
        }

        public static bool operator <(DateSpan left, DateSpan right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(DateSpan left, DateSpan right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(DateSpan left, DateSpan right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(DateSpan left, DateSpan right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
