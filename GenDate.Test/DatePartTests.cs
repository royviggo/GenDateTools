﻿using System;
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
        [InlineData(0, 0, 0, "00000000")]
        [InlineData(1900, 1, 1, "19000101")]
        [InlineData(1898, 11, 1, "18981101")]
        [InlineData(1890, 11, 21, "18901121")]
        [InlineData(2018, 02, 29, "20180229")]
        [InlineData(9999, 12, 31, "99991231")]
        public void DatePart_EqualToOtherDatePart_ReturnsTrue(int year, int month, int day, string compareDate)
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
        public void DatePart_EqualToOtherDatePartUsingOperator_ReturnsTrue(int year, int month, int day, string compareDate)
        {
            var datePart1 = new DatePart(year, month, day);
            var datePart2 = new DatePart(compareDate);

            Assert.True(datePart1 == datePart2);
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
    }
}