using GenDateTools.Parser;
using System;
using System.Collections.Generic;

namespace GenDateTools
{
    public class GenDate : IEquatable<GenDate>, IComparable<GenDate>
    {
        private const int BeforeAfterYears = 20;

        public GenDateType DateType { get; set; }
        public DatePart DateFrom { get;  set; }
        public DatePart DateTo { get;  set; }
        public string DatePhrase { get;  set; }
        public bool IsValid { get;  set; }
        public int SortDate { get;  set; }
        public string DateString => CreateDateString();
        public long DateLong => GetLongDate();

        public GenDate() { }

        public GenDate(DatePart datePart)
            : this(GenDateType.Exact, datePart, datePart, string.Empty, datePart.IsValidDate()) { }

        public GenDate(GenDateType dateType, DatePart datePart)
            : this(dateType, datePart, datePart, string.Empty, datePart.IsValidDate()) { }

        public GenDate(GenDateType dateType, DatePart fromDatePart, DatePart toDatePart)
            : this(dateType, fromDatePart, toDatePart, string.Empty, fromDatePart.IsValidDate() && toDatePart.IsValidDate()) { }

        public GenDate(GenDateType dateType, DatePart fromDatePart, DatePart toDatePart, bool isValid) 
            : this(dateType, fromDatePart, toDatePart, string.Empty, isValid) { }

        public GenDate(GenDateType dateType, DatePart fromDatePart, DatePart toDatePart, string datePhrase, bool isValid)
        {
            DateType = dateType;
            DateFrom = fromDatePart;
            DateTo = toDatePart;
            DatePhrase = datePhrase;
            IsValid = isValid;
            SortDate = GetSortDate();
        }

        public GenDate(GenDateType dateType, string datePhrase, bool isValid)
            : this(dateType, new DatePart(), new DatePart(), datePhrase, isValid) { }

        public GenDate(string dateString) : this(new DateStringParser(), dateString) { }

        public GenDate(IDateStringParser parser, string dateString)
        {
            var genDate = parser.Parse(dateString);
            DateType = genDate.DateType;
            DateFrom = genDate.DateFrom;
            DateTo = genDate.DateTo;
            DatePhrase = genDate.DatePhrase;
            IsValid = genDate.IsValid;
            SortDate = GetSortDate();
        }

        public GenDate(long dateNum)
        {
            DateType = (GenDateType)((dateNum / 100000000) % 10);
            DateFrom = new DatePart((dateNum / 1000000000) % 100000000);
            DateTo = new DatePart(dateNum % 100000000);
            DatePhrase = string.Empty;
            IsValid = true;
            SortDate = GetSortDate();
        }

        public static bool operator ==(GenDate genDate1, GenDate genDate2)
        {
            if (genDate1 is null)
                return false;

            return genDate1.Equals(genDate2);
        }

        public static bool operator !=(GenDate genDate1, GenDate genDate2)
        {
            if (genDate1 is null)
                return false;

            return !(genDate1 == genDate2);
        }

        public static bool operator >(GenDate genDate1, GenDate genDate2)
        {
            if (genDate1 is null)
                return false;

            return genDate1.CompareTo(genDate2) == 1;
        }

        public static bool operator <(GenDate genDate1, GenDate genDate2)
        {
            if (genDate1 is null)
                return false;

            return genDate1.CompareTo(genDate2) == -1;
        }

        public static bool operator >=(GenDate genDate1, GenDate genDate2)
        {
            if (genDate1 is null)
                return false;

            return genDate1.CompareTo(genDate2) >= 0;
        }

        public static bool operator <=(GenDate genDate1, GenDate genDate2)
        {
            if (genDate1 is null)
                return false;

            return genDate1.CompareTo(genDate2) <= 0;
        }

        public int CompareTo(GenDate other)
        {
            var longDate = GetLongDate();
            return longDate.CompareTo(other.GetLongDate());

        }

        public bool Equals(GenDate other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return (DateType == other.DateType && DateFrom == other.DateFrom && DateTo == other.DateTo && 
                    DatePhrase == other.DatePhrase && IsValid == other.IsValid && SortDate == other.SortDate);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (GetType() != obj.GetType())
                return false;

            return Equals((GenDate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) DateType;
                hashCode = (hashCode * 397) ^ (DateFrom?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (DateTo?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (DatePhrase?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ IsValid.GetHashCode();
                hashCode = (hashCode * 397) ^ SortDate;
                return hashCode;
            }
        }

        public override string ToString()
        {
            var typeNames = new Dictionary<int, string> { {0, ""}, {1, "Bef. "}, {2, ""}, {3, "Abt. "}, {4, "Cal. "}, { 5, "Est. " }, {6, "Bet. "}, {7, "From "}, { 8, "Int. " }, {9, "Aft. "} };
            var rangeJoin = new Dictionary<int, string> { {6, " - "}, {7, " to "} };

            if (!IsValid)
                return DatePhrase;

            var result = typeNames[(int)DateType];
            result += DateFrom.ToString();

            if (DateType == GenDateType.Between || DateType == GenDateType.Period)
                result += rangeJoin[(int)DateType] + DateTo;

            return result;
        }

        public int GetDateFromInt()
        {
            if (DateType == GenDateType.Before)
                return Convert.ToInt32(DateFrom.AddYears(-BeforeAfterYears));

            if (DateType == GenDateType.After)
                return Convert.ToInt32(DateFrom.AddDays(1));

            return Convert.ToInt32(DateFrom);
        }

        public int GetDateToInt()
        {
            if (DateType == GenDateType.About || DateType == GenDateType.Calculated || DateType == GenDateType.Estimated || DateType == GenDateType.Exact || DateType == GenDateType.Interpreted)
                return Convert.ToInt32(DatePart.GetMaxRange(DateFrom));

            if (DateType == GenDateType.Before)
                return Convert.ToInt32(DateFrom.AddDays(-1));

            if (DateType == GenDateType.After)
                return Convert.ToInt32(DatePart.GetMaxRange(DateFrom.AddYears(BeforeAfterYears)));

            return Convert.ToInt32(DatePart.GetMaxRange(DateTo));
        }

        private int GetSortDate()
        {
            return (DatePart.CompareValue(DateFrom) * 10) + (int)DateType;
        }

        private long GetLongDate()
        {
            if (!IsValid)
                return 0;

            return ((DatePart.CompareValue(DateFrom) + 100000000L) * 1000000000L) + 
                   (((long)DateType) * 100000000L) + 
                   DatePart.CompareValue(DateTo);
        }

        private string CreateDateString()
        {
            return IsValid
                ? string.Join("", new List<string> { "1", DateFrom.ToSortString(), ((int)DateType).ToString(), DateTo.ToSortString() })
                : string.Join("", new List<string> { "2", DatePhrase });
        }
    }
}