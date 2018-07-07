using System;
using Xunit;

namespace GenDate.Test
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
        [InlineData("10000000", "10000000")]
        [InlineData("19000101", "19000101")]
        [InlineData("18981101", "18981101")]
        [InlineData("18900101", "18900101")]
        [InlineData("20180229", "20180229")]
        [InlineData("99991231", "99991231")]
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

            Assert.Equal(expected, datePart.ToString());
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
        [InlineData(1900, 1, 1, true)]
        [InlineData(1898, 2, 31, false)]
        [InlineData(1890, 11, 31, false)]
        [InlineData(1980, 02, 29, true)]
        [InlineData(2000, 02, 29, true)]
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
    }
}
