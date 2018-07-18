using GenDateTools.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenDateTools
{
    public class GenDate : IEquatable<GenDate>, IComparable<GenDate>
    {
        [NotMapped]
        public IDateStringParser StringParser { get; private set; } = new DateStringParser();

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
        {
            DateFrom = datePart;
            DateTo = datePart;
            DateType = GenDateType.Exact;
            SortDate = GetSortDate();
            IsValid = true;
        }

        public GenDate(GenDateType dateType, DatePart datePart)
        {
            DateFrom = datePart;
            DateTo = datePart;
            DateType = dateType;
            SortDate = GetSortDate();
            IsValid = true;
        }

        public GenDate(GenDateType dateType, DatePart fromDatePart, DatePart toDatePart)
        {
            DateFrom = fromDatePart;
            DateTo = toDatePart;
            DateType = dateType;
            SortDate = GetSortDate();
            IsValid = true;
        }

        public GenDate(GenDateType dateType, DatePart fromDatePart, DatePart toDatePart, bool isValid)
        {
            DateType = dateType;
            DateFrom = fromDatePart;
            DateTo = toDatePart;
            SortDate = GetSortDate();
            IsValid = isValid;
        }

        public GenDate(GenDateType dateType, DatePart fromDatePart, DatePart toDatePart, string datePhrase, bool isValid)
        {
            DateType = dateType;
            DateFrom = fromDatePart;
            DateTo = toDatePart;
            DatePhrase = datePhrase;
            SortDate = GetSortDate();
            IsValid = isValid;
        }

        public GenDate(GenDateType dateType, string datePhrase, bool isValid)
        {
            DateType = dateType;
            DatePhrase = datePhrase;
            SortDate = GetSortDate();
            IsValid = isValid;
        }

        public GenDate(string dateString)
        {
            var genDate = StringParser.Parse(dateString);
            DateType = genDate.DateType;
            DateFrom = genDate.DateFrom;
            DateTo = genDate.DateTo;
            DatePhrase = genDate.DatePhrase;
            IsValid = genDate.IsValid;
            SortDate = GetSortDate();
        }

        public GenDate(IDateStringParser parser, string dateString)
        {
            StringParser = parser;
            var genDate = StringParser.Parse(dateString);
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
            SortDate = GetSortDate();
            IsValid = true;
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
            var typeNames = new Dictionary<int, string> { {0, ""}, {1, "Bef. "}, {2, ""}, {3, "Abt. "}, {4, "Cal. "}, {5, "Bet. "}, {6, "From "}, {7, "Aft. "} };
            var rangeJoin = new Dictionary<int, string> { {5, " - "}, {6, " to "} };

            if (!IsValid)
                return DatePhrase;

            var result = typeNames[(int)DateType];
            result += DateFrom.ToString();

            if (DateType == GenDateType.Between || DateType == GenDateType.FromTo)
                result += rangeJoin[(int)DateType] + DateTo;

            return result;
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