using Newtonsoft.Json;
using System;
using System.Globalization;
using Xunit;

namespace GenDateTools.Test
{
    public class DatePartTests
    {
        [Theory]
        [InlineData(10000000, "10000000")]
        [InlineData(19000101, "19000101")]
        [InlineData(18981101, "18981101")]
        [InlineData(18900101, "18900101")]
        [InlineData(20180229, "20180229")]
        [InlineData(99991231, "99991231")]
        public void DatePart_NewFromLong_ValidDatePart(long dateNum, string expected)
        {
            var datePart = new DatePart(dateNum);

            Assert.Equal(expected, datePart.ToSortString());
        }

        [Theory]
        [InlineData(2000, 0, 0, "20000000")]
        [InlineData(2000, 2, 29, "20000229")]
        [InlineData(1200, 2, 29, "12000229")]
        [InlineData(9999, 12, 31, "99991231")]
        public void DatePart_NewFromYearMonthDay_ValidDatePart(int year, int month, int day, string expected)
        {
            var datePart = new DatePart(year, month, day);

            Assert.Equal(expected, datePart.ToSortString());
        }

        [Theory]
        [InlineData(0, 0, "00000000")]
        [InlineData(2000, 0, "20000000")]
        [InlineData(2000, 60, "20000229")]
        [InlineData(1200, 29, "12000129")]
        [InlineData(9999, 365, "99991231")]
        public void DatePart_NewFromYearDayOfYear_ValidDatePart(int year, int dayOfYear, string expected)
        {
            var datePart = new DatePart(year, dayOfYear);

            Assert.Equal(expected, datePart.ToSortString());
        }

        [Theory]
        [InlineData("10000000", "10000000")]
        [InlineData("19000101", "19000101")]
        [InlineData("18981101", "18981101")]
        [InlineData("18900101", "18900101")]
        [InlineData("20180229", "20180229")]
        [InlineData("99991231", "99991231")]
        [InlineData("1890010", "18900100")]
        [InlineData("19000", "19000000")]
        [InlineData("9999", "99990000")]
        [InlineData("1", "00010000")]
        [InlineData("0", "00000000")]
        [InlineData("10000000JHDKJHDUIDANDSNAKJNDKAJLSDJA", "10000000")]
        public void DatePart_NewFromString_ValidDatePart(string dateString, string expected)
        {
            var datePart = new DatePart(dateString);

            Assert.Equal(expected, datePart.ToSortString());
        }

        [Theory]
        [InlineData(2000, 0, -1)]
        [InlineData(2000, -1, 14)]
        [InlineData(-2000, 4, 14)]
        [InlineData(2000, 14, 14)]
        [InlineData(2000, 11, 32)]
        [InlineData(10000, 0, 0)]
        public void DatePart_NewFromYearMonthDay_ThrowArgumentOutOfRangeException(int year, int month, int day)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DatePart(year, month, day));
        }

        [Theory]
        [InlineData("00000000", "10000000")]
        [InlineData("19000101", "19000102")]
        [InlineData("18981131", "18981101")]
        [InlineData("19900101", "18900101")]
        [InlineData("20180229", "20180231")]
        [InlineData("99991231", "99991230")]
        public void DatePart_NotEqualToOtherDatePartUsingOperator_ReturnsTrue(string date, string compareDate)
        {
            var datePart1 = new DatePart(date);
            var datePart2 = new DatePart(compareDate);

            Assert.True(datePart1 != datePart2);
        }

        [Theory]
        [InlineData("10000001", "10000000", 1)]
        [InlineData("19000101", "19000102", -1)]
        [InlineData("18981131", "18981131", 0)]
        [InlineData("19900112", "18900101", 1)]
        [InlineData("20180228", "20180231", -1)]
        [InlineData("99991231", "99991231", 0)]
        public void DatePart_CompareToOtherDatePart_ReturnsTrue(string date, string compareDate, int expected)
        {
            var datePart1 = new DatePart(date);
            var datePart2 = new DatePart(compareDate);

            Assert.Equal(expected, datePart1.CompareTo(datePart2));
        }

        [Theory]
        [InlineData("00010000", "1")]
        [InlineData("10000000", "1000")]
        [InlineData("19000101", "01 Jan 1900")]
        [InlineData("18981101", "01 Nov 1898")]
        [InlineData("18901200", "Dec 1890")]
        [InlineData("20180229", "29 Feb 2018")]
        [InlineData("99991231", "31 Dec 9999")]
        [InlineData("00000031", "31")]
        [InlineData("00000431", "31 Apr")]
        [InlineData("00001000", "Oct")]
        [InlineData("20170015", "15 2017")]
        public void DatePart_ToString_EqualString(string dateString, string expected)
        {
            var datePart = new DatePart(dateString);

            Assert.Equal(expected, datePart.ToString(CultureInfo.InvariantCulture));
        }

        [Theory]
        [InlineData(-20, false)]
        [InlineData(0, false)]
        [InlineData(4, true)]
        [InlineData(10, false)]
        [InlineData(100, false)]
        [InlineData(1600, true)]
        [InlineData(1900, false)]
        [InlineData(2000, true)]
        [InlineData(360, true)]
        [InlineData(2004, true)]
        [InlineData(9000, false)]
        [InlineData(10000, false)]
        public void DatePart_IsLeapYear_Expected(int year, bool expected)
        {
            var isValid = DatePart.IsLeapYear(year);

            Assert.Equal(expected, isValid);
        }

        [Theory]
        [InlineData(-0, -10, 0, false)]
        [InlineData(0, 0, 0, true)]
        [InlineData(1, 1, 0, true)]
        [InlineData(1900, 1, 1, true)]
        [InlineData(1898, 2, 31, false)]
        [InlineData(1890, 11, 31, false)]
        [InlineData(1900, 0, 10, true)]
        [InlineData(1940, 0, 0, true)]
        [InlineData(1980, 02, 29, true)]
        [InlineData(2000, 02, 29, true)]
        [InlineData(2018, 0, 31, true)]
        [InlineData(9999, 12, 31, true)]
        public void DatePart_YearMonthDay_IsValidDate(int year, int month, int day, bool expected)
        {
            var isValid = DatePart.IsValidDate(year, month, day);

            Assert.Equal(expected, isValid);
        }

        [Theory]
        [InlineData(-0, -10, 0, false)]
        [InlineData(0, 0, 0, false)]
        [InlineData(1900, 1, 1, true)]
        [InlineData(1898, 2, 31, false)]
        [InlineData(1890, 11, 31, false)]
        [InlineData(1980, 02, 29, true)]
        [InlineData(2000, 02, 29, true)]
        [InlineData(9999, 12, 31, true)]
        public void DatePart_YearMonthDay_IsValidDateTime(int year, int month, int day, bool expected)
        {
            var isValid = DatePart.IsValidDateTime(year, month, day);

            Assert.Equal(expected, isValid);
        }

        [Theory]
        [InlineData(0, 0, 31, 31)]
        [InlineData(2000, 2, 29, 60)]
        [InlineData(1900, 3, 1, 60)]
        [InlineData(2018, 1, 31, 31)]
        [InlineData(2018, 7, 31, 212)]
        [InlineData(2018, 9, 30, 273)]
        [InlineData(2020, 2, 29, 60)]
        [InlineData(2020, 12, 31, 366)]
        public void DatePart_DayOfYear_IsValidDay(int year, int month, int day, int expected)
        {
            var datePart = new DatePart(year, month, day);

            Assert.Equal(expected, datePart.DayOfYear());
        }

        [Theory]
        [InlineData(0, 0, 31)]
        [InlineData(2000, 2, 29)]
        [InlineData(1900, 2, 28)]
        [InlineData(2018, 1, 31)]
        [InlineData(2018, 7, 31)]
        [InlineData(2018, 9, 30)]
        [InlineData(2020, 2, 29)]
        [InlineData(2020, 12, 31)]
        public void DatePart_DaysInMonth_IsValidDays(int year, int month, int expected)
        {
            var daysInMonth = DatePart.DaysInMonth(year, month);

            Assert.Equal(expected, daysInMonth);
        }

        [Theory]
        [InlineData(0, 365)]
        [InlineData(1, 365)]
        [InlineData(4, 366)]
        [InlineData(100, 365)]
        [InlineData(1900, 365)]
        [InlineData(2000, 366)]
        [InlineData(2018, 365)]
        [InlineData(2020, 366)]
        [InlineData(2400, 366)]
        [InlineData(9999, 365)]
        public void DatePart_DaysInYear_IsValidDays(int year, int expected)
        {
            var daysInYear = DatePart.DaysInYear(year);

            Assert.Equal(expected, daysInYear);
        }

        [Theory]
        [InlineData(0, 0, 0, "00000000")]
        [InlineData(1900, 1, 1, "19000101")]
        [InlineData(1898, 11, 1, "18981101")]
        [InlineData(1890, 11, 21, "18901121")]
        [InlineData(2018, 02, 29, "20180229")]
        [InlineData(9999, 12, 31, "99991231")]
        public void DatePart_Equals_ReturnsTrue(int year, int month, int day, string compareDate)
        {
            var datePart1 = new DatePart(year, month, day);
            var datePart2 = new DatePart(compareDate);

            Assert.True(datePart1.Equals(datePart2));
        }

        [Theory]
        [InlineData(0, 0, 0, "00000000")]
        [InlineData(1900, 1, 1, "19000101")]
        [InlineData(1898, 11, 1, "18981101")]
        [InlineData(1890, 11, 21, "18901121")]
        [InlineData(2018, 02, 29, "20180229")]
        [InlineData(9999, 12, 31, "99991231")]
        public void DatePart_OperatorEqual_ReturnsTrue(int year, int month, int day, string compareDate)
        {
            var datePart1 = new DatePart(year, month, day);
            var datePart2 = new DatePart(compareDate);

            Assert.True(datePart1 == datePart2);
        }

        [Theory]
        [InlineData(0, 0, 0, "00000000")]
        [InlineData(1900, 1, 1, "19000101")]
        [InlineData(1898, 11, 1, "18981101")]
        [InlineData(1890, 11, 21, "18901121")]
        [InlineData(2018, 02, 29, "20180229")]
        [InlineData(9999, 12, 31, "99991231")]
        public void DatePart_OperatorNotEqual_ReturnsTrue(int year, int month, int day, string compareDate)
        {
            var datePart1 = new DatePart(year, month, day);
            var datePart2 = new DatePart(compareDate);

            Assert.False(datePart1 != datePart2);
        }

        [Theory]
        [InlineData(0, 0, 1, "00000000")]
        [InlineData(1900, 1, 2, "19000101")]
        [InlineData(1899, 11, 1, "18981101")]
        [InlineData(1890, 12, 21, "18901121")]
        [InlineData(2018, 03, 29, "20180229")]
        [InlineData(9999, 12, 31, "99991230")]
        public void DatePart_OperatorLargerThan_ReturnsTrue(int year, int month, int day, string compareDate)
        {
            var datePart1 = new DatePart(year, month, day);
            var datePart2 = new DatePart(compareDate);

            Assert.True(datePart1 > datePart2);
        }

        [Theory]
        [InlineData(0, 0, 0, "00000001")]
        [InlineData(1900, 1, 1, "19000102")]
        [InlineData(1898, 11, 1, "18981201")]
        [InlineData(1890, 11, 21, "18901122")]
        [InlineData(2018, 02, 29, "20180329")]
        [InlineData(9999, 12, 30, "99991231")]
        public void DatePart_OperatorLessThan_ReturnsTrue(int year, int month, int day, string compareDate)
        {
            var datePart1 = new DatePart(year, month, day);
            var datePart2 = new DatePart(compareDate);

            Assert.True(datePart1 < datePart2);
        }

        [Theory]
        [InlineData(0, 0, 0, "00000000")]
        [InlineData(1900, 1, 2, "19000101")]
        [InlineData(1899, 11, 1, "18981101")]
        [InlineData(1890, 12, 21, "18901121")]
        [InlineData(2018, 02, 29, "20180229")]
        [InlineData(9999, 12, 31, "99991231")]
        public void DatePart_OperatorLargerThanOrEqual_ReturnsTrue(int year, int month, int day, string compareDate)
        {
            var datePart1 = new DatePart(year, month, day);
            var datePart2 = new DatePart(compareDate);

            Assert.True(datePart1 >= datePart2);
        }

        [Theory]
        [InlineData(0, 0, 0, "00000000")]
        [InlineData(1900, 1, 1, "19000102")]
        [InlineData(1898, 11, 1, "18981201")]
        [InlineData(1890, 11, 21, "18901122")]
        [InlineData(2018, 02, 29, "20180229")]
        [InlineData(9999, 12, 31, "99991231")]
        public void DatePart_OperatorLessThanOrEqual_ReturnsTrue(int year, int month, int day, string compareDate)
        {
            var datePart1 = new DatePart(year, month, day);
            var datePart2 = new DatePart(compareDate);

            Assert.True(datePart1 <= datePart2);
        }

        [Fact]
        public void DatePart_ConvertToBoolean_ThrowsInvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() => Convert.ToBoolean(new DatePart(20000101)));
        }

        [Fact]
        public void DatePart_ConvertToInt16_ThrowsInvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() => Convert.ToInt16(new DatePart(20000101)));
        }

        [Fact]
        public void DatePart_ConvertToByte_ThrowsInvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() => Convert.ToByte(new DatePart(20000101)));
        }

        [Fact]
        public void DatePart_ConvertToChar_ThrowsInvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() => Convert.ToChar(new DatePart(20000101)));
        }

        [Fact]
        public void DatePart_ConvertToSByte_ThrowsInvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() => Convert.ToSByte(new DatePart(20000101)));
        }

        [Fact]
        public void DatePart_ConvertToUInt16_ThrowsInvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() => Convert.ToUInt16(new DatePart(20000101)));
        }

        [Fact]
        public void DatePart_ConvertToInt32_IsEqualToType()
        {
            var datePartConverted = Convert.ToInt32(new DatePart(20000101));
            var expected = Convert.ToInt32(20000101);

            Assert.Equal(expected, datePartConverted);
            Assert.IsType<Int32>(datePartConverted);
        }

        [Fact]
        public void DatePart_ConvertToInt64_IsEqualToType()
        {
            var datePartConverted = Convert.ToInt64(new DatePart(20000101));
            var expected = Convert.ToInt64(20000101);

            Assert.Equal(expected, datePartConverted);
            Assert.IsType<Int64>(datePartConverted);
        }

        [Fact]
        public void DatePart_ConvertToUInt32_IsEqualToType()
        {
            var datePartConverted = Convert.ToUInt32(new DatePart(20000101));
            var expected = Convert.ToUInt32(20000101);

            Assert.Equal(expected, datePartConverted);
            Assert.IsType<UInt32>(datePartConverted);
        }

        [Fact]
        public void DatePart_ConvertToUInt64_IsEqualToType()
        {
            var datePartConverted = Convert.ToUInt64(new DatePart(20000101));
            var expected = Convert.ToUInt64(20000101);

            Assert.Equal(expected, datePartConverted);
            Assert.IsType<UInt64>(datePartConverted);
        }

        [Fact]
        public void DatePart_ConvertToSingle_IsEqualToType()
        {
            var datePartConverted = Convert.ToSingle(new DatePart(20000101));
            var expected = Convert.ToSingle(20000101);

            Assert.Equal(expected, datePartConverted);
            Assert.IsType<Single>(datePartConverted);
        }

        [Fact]
        public void DatePart_ConvertToDouble_IsEqualToType()
        {
            var datePartConverted = Convert.ToDouble(new DatePart(20000101));
            var expected = Convert.ToDouble(20000101);

            Assert.Equal(expected, datePartConverted);
            Assert.IsType<Double>(datePartConverted);
        }

        [Fact]
        public void DatePart_ConvertToDecimal_IsEqualToType()
        {
            var datePartConverted = Convert.ToDecimal(new DatePart(20000101));
            var expected = Convert.ToDecimal(20000101);

            Assert.Equal(expected, datePartConverted);
            Assert.IsType<Decimal>(datePartConverted);
        }

        [Theory]
        [InlineData("10000000", "1000")]
        [InlineData("19000101", "01 Jan 1900")]
        [InlineData("18981101", "01 Nov 1898")]
        [InlineData("18901200", "Dec 1890")]
        [InlineData("20180229", "29 Feb 2018")]
        [InlineData("99991231", "31 Dec 9999")]
        [InlineData("00000031", "31")]
        [InlineData("00000431", "31 Apr")]
        [InlineData("00001000", "Oct")]
        [InlineData("20170015", "15 2017")]
        public void DatePart_ConvertToString_IsStringAndEqual(string dateString, string expected)
        {
            var datePartConverted = Convert.ToString(new DatePart(dateString));

            Assert.Equal(expected, datePartConverted);
            Assert.IsType<string>(datePartConverted);
        }

        [Theory]
        [InlineData(0, 1, 10000)]
        [InlineData(10000000, 1000, 20000000)]
        [InlineData(20000229, 4, 20040229)]
        [InlineData(20000229, 100, 21000229)]
        [InlineData(99981231, 1, 99991231)]
        [InlineData(10000, -1, 0)]
        [InlineData(10000000, -1000, 0)]
        [InlineData(20000229, -4, 19960229)]
        [InlineData(20000229, -100, 19000229)]
        [InlineData(99991231, -1, 99981231)]
        public void DatePart_AddYear_DatePartWithAddedYears(long datePartLong, int years, long expected)
        {
            var datePart = new DatePart(datePartLong);
            var datePartAdded = datePart.AddYears(years);
            var datePartExpected = new DatePart(expected);

            Assert.Equal(datePartExpected, datePartAdded);
        }

        [Theory]
        [InlineData(99991231, 1)]
        [InlineData(99981231, 2)]
        [InlineData(1, 99991231)]
        [InlineData(99991231, -100000000)]
        public void DatePart_AddYear_ThrowsArgumentOutOfRangeException(long datePartLong, int years)
        {
            var datePart = new DatePart(datePartLong);

            Assert.Throws<ArgumentOutOfRangeException>(() => datePart.AddYears(years));
        }

        [Theory]
        [InlineData(100, 1, 200)]
        [InlineData(900, 1, 1000)]
        [InlineData(1000, 1, 1100)]
        [InlineData(10000, 1, 10100)]
        [InlineData(19000000, 1000, 19830400)]
        [InlineData(19040229, -48, 19000228)]
        [InlineData(20000131, 1, 20000229)]
        [InlineData(20000228, 12, 20010228)]
        [InlineData(20040229, -12, 20030228)]
        [InlineData(20040229, -48, 20000229)]
        [InlineData(99991231, -1, 99991130)]
        public void DatePart_AddMonths_DatePartWithAddedMonths(long datePartLong, int months, long expected)
        {
            var datePart = new DatePart(datePartLong);
            var datePartAdded = datePart.AddMonths(months);
            var datePartExpected = new DatePart(expected);

            Assert.Equal(datePartExpected, datePartAdded);
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(1, -2)]
        [InlineData(20000000, 100000)]
        [InlineData(99991231, 1)]
        [InlineData(99991231, -100000000)]
        public void DatePart_AddMonths_ThrowsArgumentOutOfRangeException(long datePartLong, int years)
        {
            var datePart = new DatePart(datePartLong);

            Assert.Throws<ArgumentOutOfRangeException>(() => datePart.AddMonths(years));
        }

        [Theory]
        [InlineData(10000, 1, 10101)]
        [InlineData(19000000, 366, 19010101)]
        [InlineData(20000131, 1, 20000201)]
        [InlineData(20000229, 2, 20000302)]
        [InlineData(20000101, -1, 19991231)]
        [InlineData(20000301, -1, 20000229)]
        [InlineData(99991231, -1, 99991230)]
        [InlineData(20010101, -367, 19991231)]
        public void DatePart_AddDays_DatePartWithAddedDays(long datePartLong, int days, long expected)
        {
            var datePart = new DatePart(datePartLong);
            var datePartAdded = datePart.AddDays(days);
            var datePartExpected = new DatePart(expected);

            Assert.Equal(datePartExpected, datePartAdded);
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(1, -2)]
        [InlineData(90000000, 370000)]
        [InlineData(99991231, 1)]
        public void DatePart_AddDays_ThrowsArgumentOutOfRangeException(long datePartLong, int days)
        {
            var datePart = new DatePart(datePartLong);

            Assert.Throws<ArgumentOutOfRangeException>(() => datePart.AddDays(days));
        }

        [Fact]
        public void DatePart_Serialize_ReturnsValidObject()
        {
            var datePart = new DatePart(2000, 1, 1);
            var expected = "{\"Year\":2000,\"Month\":1,\"Day\":1}";
            var actual = JsonConvert.SerializeObject(datePart);

            Assert.Equal(expected, actual);
        }
    }
}
