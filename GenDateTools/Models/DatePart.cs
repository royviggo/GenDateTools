using System;
using System.Runtime.Serialization;

namespace GenDateTools
{
    /// <summary>
    /// <para>DatePart is a class for genealogy dates. It has properties for Year, Month and Day, and anyone of them can be 0, which means 
    /// that the value is unknown.</para><para>Limitations: Uses rules from the Gregorian calendar, e.g. leap year. Year must be a number 
    /// from 0 to 9999, Month a number from 0 to 12 and Day a number between 0 and 31.</para>
    /// </summary>
    [Serializable]
    public class DatePart : IEquatable<DatePart>, IComparable<DatePart>, IConvertible, ISerializable
    {
        internal const int _maxDaysInMonth = 31;
        internal const int _minDaysInMonth = 0;
        internal const int _maxMonthInYear = 12;
        internal const int _minMonthInYear = 0;
        private const int _maxYear = 9999;
        private const int _minYear = 0;

        public static readonly DatePart MinValue = new DatePart(_minYear, _minMonthInYear, _minDaysInMonth);
        public static readonly DatePart MaxValue = new DatePart(_maxYear, _maxMonthInYear, _maxDaysInMonth);

        private static readonly int[] _daysToMonth365 = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
        private static readonly int[] _daysToMonth366 = { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }

        /// <summary>
        /// Creates a DatePart object without parameters, with only unknown values for Year, Month and Day.
        /// </summary>
        public DatePart()
        {
            Year = MinValue.Year;
            Month = MinValue.Month;
            Day = MinValue.Day;
        }

        /// <summary>
        /// Creates a DatePart object using three parameters; year, month and day, similar to DateTime. year must be an integer 
        /// between 0 and 9999, month an integer between 0 and 12, and day an integer between 0 and 31. <para>Any combinations of these 
        /// values work, even if the date is not valid. The value 0 means that it is unknown. Throws an exception if parameters are out 
        /// of range.</para>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DatePart(int year, int month, int day)
        {
            if (!(year >= _minYear && year <= _maxYear && month >= _minMonthInYear && month <= _maxMonthInYear && day >= _minDaysInMonth && day <= _maxDaysInMonth))
            {
                throw new ArgumentOutOfRangeException("Arguments out of range");
            }

            Year = year;
            Month = month;
            Day = day;
        }

        /// <summary>
        /// Creates a DatePart object using 3 string parameters; year, month and day. 
        /// See also: <seealso cref="DatePart.DatePart(int, int, int)"/>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DatePart(string year, string month, string day)
            : this(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day)) { }

        /// <summary>
        /// Creates a DatePart object using a numeric date as a parameter. It is a long between 0 and 99991231, which is made with the formula:
        /// <para><c>Year * 10000 + Month * 100 + Day</c></para>
        /// <para>These values are recreated from the long value, and passed to <see cref="DatePart.DatePart(int, int, int)"/>.</para>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DatePart(long date)
            : this((int)date / 10000, (int)(date / 100) % 100, (int)date % 100) { }

        /// <summary>
        /// Creates a DatePart object using a string date of the format <c>YYYYMMDD</c>. The values are split and passed to 
        /// <see cref="DatePart.DatePart(int, int, int)"/>. If the string is shorter than 8 numbers, it converts the first 
        /// 4 numbers to year, and the next 2 to month if it's 6 characters long.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        public DatePart(string date)
        {
            Year = (date.Length >= 4) ? Convert.ToInt32(date.Substring(0, 4)) : Convert.ToInt32(date);
            Month = (date.Length >= 6) ? Convert.ToInt32(date.Substring(4, 2)) : 0;
            Day = (date.Length == 8) ? Convert.ToInt32(date.Substring(6, 2)) : 0;
        }

        protected DatePart(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Year = Convert.ToInt32(info.GetString("Year"));
            Month = Convert.ToInt32(info.GetString("Month"));
            Day = Convert.ToInt32(info.GetString("Day"));
        }

        /// <summary>
        /// Creates a DatePart object using a year and the day number in that year.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DatePart(int year, int dayOfYear)
        {
            if (!(year >= _minYear && year <= _maxYear))
            {
                throw new ArgumentOutOfRangeException(nameof(year), year, "Arguments out of range");
            }

            if (!(dayOfYear >= 0 && dayOfYear <= DaysInYear(year)))
            {
                throw new ArgumentOutOfRangeException(nameof(dayOfYear), dayOfYear, "Arguments out of range");
            }

            var month = 0;
            int[] daysInMonth = IsLeapYear(year) ? _daysToMonth366 : _daysToMonth365;

            while (dayOfYear > daysInMonth[month])
            {
                month++;
                if (month == 13)
                {
                    break;
                }
            }

            Year = year;
            Month = month > 12 ? month - 1 : month;
            Day = dayOfYear - daysInMonth[month > 0 ? month - 1 : 0];
        }

        /// <summary>
        /// Gets the day number from the beginning of the year.
        /// </summary>
        public int DayOfYear()
        {
            int[] daysInMonth = IsLeapYear(Year) ? _daysToMonth366 : _daysToMonth365;
            var prevMonth = Month > 0 ? Month - 1 : 0;

            return daysInMonth[prevMonth] + Day;
        }

        /// <summary>
        /// Gets the number of days in the month.
        /// </summary>
        public int DaysInMonth()
        {
            return DaysInMonth(Year, Month);
        }

        /// <summary>
        /// Checks if the DatePart object is valid, which means a valid date in the Gregorian calendar, except that 
        /// one or more of the values Year, Month and Day can be 0. If a value is 0, it means that it is unknown.
        /// </summary>
        public bool IsValidDate()
        {
            return IsValidDate(Year, Month, Day);
        }

        /// <summary>
        /// Check if the DatePart object is a valid DateTime.
        /// </summary>
        public bool IsValidDateTime()
        {
            return IsValidDateTime(Year, Month, Day);
        }

        /// <summary>
        /// Check if the year is a leap year.
        /// </summary>
        public bool IsLeapYear()
        {
            return IsLeapYear(Year);
        }

        /// <summary>
        /// Adds a number of years to the DatePart.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DatePart AddYears(int years)
        {
            if (((Year + years) > _maxYear) || ((Year + years) < _minYear))
            {
                throw new ArgumentOutOfRangeException(nameof(years), years, $"Resulting year must be between {_minYear} and {_maxYear}.");
            }

            return new DatePart(Year + years, Month, Day);
        }

        /// <summary>
        /// Adds a number of months to the DatePart.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DatePart AddMonths(int months)
        {
            int year = Year, mon = Month, day = Day;

            var calcMonth = mon - 1 + months;
            if (calcMonth >= 0)
            {
                mon = (calcMonth % 12) + 1;
                year += calcMonth / 12;
            }
            else
            {
                mon = 12 + ((calcMonth + 1) % 12);
                year += (calcMonth - 11) / 12;
            }

            if (year < _minYear || year > _maxYear)
            {
                throw new ArgumentOutOfRangeException(nameof(months), months, $"Resulting year must be between {_minYear} and {_maxYear}.");
            }

            var daysInMonth = DaysInMonth(year, mon);
            if (day > daysInMonth)
            {
                day = daysInMonth;
            }

            return new DatePart(year, mon, day);
        }

        /// <summary>
        /// Adds a number of days to the DatePart.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DatePart AddDays(int days)
        {
            int year = Year;

            var dayOfYear = DayOfYear();
            var calcDayOfYear = dayOfYear + days;

            if (calcDayOfYear > 0)
            {
                while (calcDayOfYear > DaysInYear(year))
                {
                    calcDayOfYear -= DaysInYear(year);
                    year++;

                    if (year > _maxYear)
                    {
                        throw new ArgumentOutOfRangeException(nameof(days), $"Resulting year must be between {_minYear} and {_maxYear}.");
                    }
                }
            }
            else
            {
                calcDayOfYear = calcDayOfYear > 0 ? 0 - calcDayOfYear : calcDayOfYear;
                while (calcDayOfYear < 1)
                {
                    year--;

                    if (year < _minYear)
                    {
                        throw new ArgumentOutOfRangeException(nameof(days), $"Resulting year must be between {_minYear} and {_maxYear}.");
                    }

                    calcDayOfYear += DaysInYear(year);
                }
            }

            return new DatePart(year, calcDayOfYear);
        }

        /// <summary>
        /// Gets the number of days in a month, given a year and a month.
        /// </summary>
        public static int DaysInMonth(int year, int month)
        {
            int[] daysInMonth = IsLeapYear(year) ? _daysToMonth366 : _daysToMonth365;

            return month == 0 ? _maxDaysInMonth : daysInMonth[month] - daysInMonth[month - 1];
        }

        /// <summary>
        /// Gets the number of days in a given year.
        /// </summary>
        public static int DaysInYear(int year)
        {
            int[] daysInMonth = IsLeapYear(year) ? _daysToMonth366 : _daysToMonth365;

            return daysInMonth[daysInMonth.Length - 1];
        }

        /// <summary>
        /// Checks if the parameters year, month and day is a valid DatePart, which means a valid date in the Gregorian 
        /// calendar, except that one or more of the values can be 0. If a value is 0, it means that it is unknown.
        /// </summary>
        public static bool IsValidDate(int year, int month, int day)
        {
            return year >= _minYear && year <= _maxYear && month >= _minMonthInYear && month <= _maxMonthInYear &&
                   day >= _minDaysInMonth && day <= DaysInMonth(year, month);
        }

        /// <summary>
        /// Checks if the parameters form a valid DateTime.
        /// </summary>
        public static bool IsValidDateTime(int year, int month, int day)
        {
            return year >= 1 && month >= 1 && day >= 1 && IsValidDate(year, month, day);
        }

        /// <summary>
        /// Check if a year is a leap year.
        /// </summary>
        public static bool IsLeapYear(int year)
        {
            if (year <= _minYear || year > _maxYear)
            {
                return false;
            }

            return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
        }

        /// <summary>
        /// Gets the current date as a DatePart.
        /// </summary>
        public static DatePart Today()
        {
            var dateTime = DateTime.Today;

            return new DatePart(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public int CompareTo(DatePart other)
        {
            var sortDate = CompareValue(this);
            var sortDateOther = CompareValue(other);

            return sortDate.CompareTo(sortDateOther);
        }

        public static int CompareValue(DatePart datePart)
        {
            return datePart != null ? ((datePart.Year * 10000) + (datePart.Month * 100) + datePart.Day) : 0;
        }

        public static bool operator ==(DatePart datePart1, DatePart datePart2)
        {
            if (datePart1 is null)
            {
                return false;
            }

            return datePart1.Equals(datePart2);
        }

        public static bool operator !=(DatePart datePart1, DatePart datePart2)
        {
            if (datePart1 is null)
            {
                return false;
            }

            return !(datePart1 == datePart2);
        }

        public static bool operator >(DatePart datePart1, DatePart datePart2)
        {
            if (datePart1 is null)
            {
                return false;
            }

            return datePart1.CompareTo(datePart2) == 1;
        }

        public static bool operator <(DatePart datePart1, DatePart datePart2)
        {
            if (datePart1 is null)
            {
                return false;
            }

            return datePart1.CompareTo(datePart2) == -1;
        }

        public static bool operator >=(DatePart datePart1, DatePart datePart2)
        {
            if (datePart1 is null)
            {
                return false;
            }

            return datePart1.CompareTo(datePart2) >= 0;
        }

        public static bool operator <=(DatePart datePart1, DatePart datePart2)
        {
            if (datePart1 is null)
            {
                return false;
            }

            return datePart1.CompareTo(datePart2) <= 0;
        }

        public bool Equals(DatePart other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (Year == other.Year && Month == other.Month && Day == other.Day);
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

            return Equals((DatePart)obj);
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

        /// <summary>
        /// Converts a DatePart to a string representation with the format <c>01 Jan 2000</c>.
        /// </summary>
        public override string ToString()
        {
            var output = Day > 0 && Day <= _maxDaysInMonth ? Day.ToString().PadLeft(2, '0') : "";
            var month = Month > 0 && Month <= _maxMonthInYear ? GenTools.MonthName(Month) : "";
            var year = Year > 0 ? Year.ToString() : "";

            output += output != "" && month != "" ? " " : "";
            output += month;
            output += output != "" && year != "" ? " " : "";

            return output + year;
        }

        #region IConvertible GetTypeCode and Methods
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return (IsValidDateTime()) ? new DateTime(Year, Month, Day) : new DateTime();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(CompareValue(this));
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(CompareValue(this));
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(CompareValue(this));
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(CompareValue(this));
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(CompareValue(this));
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(CompareValue(this), conversionType);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(CompareValue(this));
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(CompareValue(this));
        }
        #endregion

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Year", Year);
            info.AddValue("Month", Month);
            info.AddValue("Day", Day);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            GetObjectData(info, context);
        }

        internal static DatePart GetMaxRange(DatePart datePart)
        {
            return new DatePart(datePart.Year,
                    datePart.Month != 0 ? datePart.Month : _maxMonthInYear,
                    datePart.Day != 0 ? datePart.Day : DaysInMonth(datePart.Year, datePart.Month));
        }
    }
}