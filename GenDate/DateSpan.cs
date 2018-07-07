using System;

namespace GenDate
{
    public class DateSpan : IEquatable<DateSpan>, IComparable<DateSpan>
    {
        private const int DaysPerYear = 365;

        internal int _years;
        internal int _days;
        internal int _totalDays;

        public DateSpan(int totalDays)
        {
            _totalDays = totalDays;
            _years = totalDays / DaysPerYear;
            _days = totalDays % DaysPerYear;
        }

        public DateSpan(int years, int days)
        {
            _years = years;
            _days = days;
            _totalDays = years * DaysPerYear + days;
        }

        public int Days => _days;
        public int Years => _years;
        public int TotalDays => _totalDays;

        public static int Compare(DateSpan dateSpan1, DateSpan dateSpan2)
        {
            if (dateSpan1.TotalDays > dateSpan2.TotalDays) return 1;
            if (dateSpan1.TotalDays < dateSpan2.TotalDays) return -1;
            return 0;
        }

        public int CompareTo(DateSpan other)
        {
            return Compare(this, other);
        }

        public bool Equals(DateSpan other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return (TotalDays == other.TotalDays);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (GetType() != obj.GetType())
                return false;

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
            var days = $"{Days} year{(Days == 0 || Days > 1 ? "s" : "")}";

            var output = Years > 0 ? years : "";
            output += (Years > 0 && Days > 0) ? " and " : "";

            return output + days;
        }

    }
}
