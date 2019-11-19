﻿using GenDateTools.Parser;
using System;
using System.Collections.Generic;

namespace GenDateTools
{
    /// <summary>
    /// GenDate is a class for genealogy dates, with support for date ranges and specification af the date precision and if an event
    /// happened before or after a date. There is also support for free text. Every date is essentially a date range.
    /// </summary>
    public class GenDate : IEquatable<GenDate>, IComparable<GenDate>
    {
        private const int BeforeAfterYears = 20;

        public GenDate() { }

        /// <summary>
        /// Create a new GenDate from a single DatePart.
        /// </summary>
        /// <param name="datePart">A DatePart that represent the from and to part of the date range. See <seealso cref="DatePart"/>.</param>
        public GenDate(DatePart datePart)
            : this(GenDateType.Exact, datePart, datePart, string.Empty, datePart.IsValidDate()) { }

        /// <summary>
        /// Create a new GenDate from a date type and a DatePart.
        /// </summary>
        /// <param name="dateType">The date type, see <seealso cref="GenDateType"/>.</param>
        /// <param name="datePart">A DatePart that represent the from and to part of the date range. See <seealso cref="DatePart"/>.</param>
        public GenDate(GenDateType dateType, DatePart datePart)
            : this(dateType, datePart, datePart, string.Empty, datePart.IsValidDate()) { }

        /// <summary>
        /// Create a new GenDate from a set of parameters.
        /// </summary>
        /// <param name="dateType">The date type, see <seealso cref="GenDateType"/>.</param>
        /// <param name="fromDatePart">A DatePart that represent the from part of the date range. See <seealso cref="DatePart"/>.</param>
        /// <param name="toDatePart">A DatePart that represent the to part of the date range. Can be equal to or larger than fromDatePart.</param>
        public GenDate(GenDateType dateType, DatePart fromDatePart, DatePart toDatePart)
            : this(dateType, fromDatePart, toDatePart, string.Empty, fromDatePart.IsValidDate() && toDatePart.IsValidDate()) { }

        public GenDate(GenDateType dateType, DatePart fromDatePart, DatePart toDatePart, bool isValid)
            : this(dateType, fromDatePart, toDatePart, string.Empty, isValid) { }

        /// <summary>
        /// Create a new GenDate from a set of parameters.
        /// </summary>
        /// <param name="dateType">The date type, see <seealso cref="GenDateType"/>.</param>
        /// <param name="fromDatePart">A DatePart that represent the from part of the date range. See <seealso cref="DatePart"/>.</param>
        /// <param name="toDatePart">A DatePart that represent the to part of the date range. Can be equal to or larger than fromDatePart.</param>
        /// <param name="datePhrase">Free text.</param>
        /// <param name="isValid">A boolean value to indicate if this is a valid date or not.</param>
        public GenDate(GenDateType dateType, DatePart fromDatePart, DatePart toDatePart, string datePhrase, bool isValid)
        {
            DateType = dateType;
            DateFrom = fromDatePart;
            DateTo = toDatePart;
            DatePhrase = datePhrase;
            IsValid = isValid;
        }

        /// <summary>
        /// Create a new GenDate from a set of parameters, a type, a phrase and if it's valid. No DateParts are used.
        /// </summary>
        /// <param name="dateType">The date type, see <seealso cref="GenDateType"/>.</param>
        /// <param name="datePhrase">Free text.</param>
        /// <param name="isValid">A boolean value to indicate if this is a valid date or not.</param>
        public GenDate(GenDateType dateType, string datePhrase, bool isValid)
            : this(dateType, new DatePart(), new DatePart(), datePhrase, isValid) { }

        /// <summary>
        /// Create a new GenDate from a string that get parsed. 
        /// </summary>
        /// <param name="dateString">Format: string type | from date part | date type | to date part</param>
        public GenDate(string dateString) : this(new DateStringParser(), dateString) { }

        /// <summary>
        /// Create a new GenDate from a string that get parsed. 
        /// </summary>
        /// <param name="parser">An IDateStringParser instance object.</param>
        /// <param name="dateString">Format: string type | from date part | date type | to date part</param>
        public GenDate(IDateStringParser parser, string dateString)
        {
            var genDate = parser.Parse(dateString);
            DateType = genDate.DateType;
            DateFrom = genDate.DateFrom;
            DateTo = genDate.DateTo;
            DatePhrase = genDate.DatePhrase;
            IsValid = genDate.IsValid;
        }

        /// <summary>
        /// Create a new GenDate from a long that contains a GenDate without a date phrase. 
        /// </summary>
        /// <param name="dateNum">First digit is string type - it should be 1. Next 8 digits is from date part. Next digit is date type,
        /// See <see cref="GenDateType"/>. Last 8 digits is to date part.</param>
        public GenDate(long dateNum)
        {
            DateType = (GenDateType)((dateNum / 100000000) % 10);
            DateFrom = new DatePart((dateNum / 1000000000) % 100000000);
            DateTo = new DatePart(dateNum % 100000000);
            DatePhrase = string.Empty;
            IsValid = true;
        }

        /// <summary>Gets or sets the type of the date.</summary>
        /// <value>The type of the date</value>
        public GenDateType DateType { get; set; }

        /// <summary>Gets or sets the from date in a date sequence.</summary>
        /// <value>The from date</value>
        public DatePart DateFrom { get; set; }

        /// <summary>Gets or sets the to date in a date sequence.</summary>
        /// <value>The to date</value>
        public DatePart DateTo { get; set; }

        /// <summary>Gets or sets the date phrase.</summary>
        /// <value>Textual value of the date</value>
        public string DatePhrase { get; set; }

        /// <summary>Gets or sets if the date is a valid GenDate or not.</summary>
        /// <value>True or false</value>
        public bool IsValid { get; set; }

        public long DateLong
        {
            get
            {
                if (!IsValid)
                    return 0;

                return ((DatePart.CompareValue(DateFrom) + 100000000L) * 1000000000L) +
                       (((long)DateType) * 100000000L) +
                       DatePart.CompareValue(DateTo);
            }
        }

        public string DateString => IsValid
                ? string.Join("", new List<string> { "1", DateFrom.ToSortString(), ((int)DateType).ToString(), DateTo.ToSortString() })
                : string.Join("", new List<string> { "2", DatePhrase });

        public int From
        {
            get
            {
                if (DateType == GenDateType.Before)
                    return Convert.ToInt32(DateFrom.AddYears(-BeforeAfterYears));

                if (DateType == GenDateType.After)
                    return Convert.ToInt32(DateFrom.AddDays(1));

                return Convert.ToInt32(DateFrom);
            }
        }

        public int To
        {
            get
            {
                if (DateType == GenDateType.About || DateType == GenDateType.Calculated || DateType == GenDateType.Estimated || DateType == GenDateType.Exact || DateType == GenDateType.Interpreted)
                    return Convert.ToInt32(DatePart.GetMaxRange(DateFrom));

                if (DateType == GenDateType.Before)
                    return Convert.ToInt32(DateFrom.AddDays(-1));

                if (DateType == GenDateType.After)
                    return Convert.ToInt32(DatePart.GetMaxRange(DateFrom.AddYears(BeforeAfterYears)));

                return Convert.ToInt32(DatePart.GetMaxRange(DateTo));
            }
        }

        public int SortDate => (DatePart.CompareValue(DateFrom) * 10) + (int)DateType;


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
            var longDate = DateLong;
            return longDate.CompareTo(other.DateLong);

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

            return Equals((GenDate)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)DateType;
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
            var typeNames = new Dictionary<int, string> { { 0, "" }, { 1, "Bef. " }, { 2, "" }, { 3, "Abt. " }, { 4, "Cal. " }, { 5, "Est. " }, { 6, "Bet. " }, { 7, "From " }, { 8, "Int. " }, { 9, "Aft. " } };
            var rangeJoin = new Dictionary<int, string> { { 6, " - " }, { 7, " to " } };

            if (!IsValid)
                return DatePhrase;

            var result = typeNames[(int)DateType];
            result += DateFrom.ToString();

            if (DateType == GenDateType.Between || DateType == GenDateType.Period)
                result += rangeJoin[(int)DateType] + DateTo;

            return result;
        }

    }
}