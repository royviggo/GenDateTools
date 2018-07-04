using Xunit;

namespace GenDate.Test
{
    public class GenExtensionTests
    {
        [Theory]
        [InlineData(-20, false)]
        [InlineData(0, false)]
        [InlineData(4, true)]
        [InlineData(10, false)]
        [InlineData(100, false)]
        [InlineData(1600, true)]
        [InlineData(365, false)]
        [InlineData(2004, true)]
        [InlineData(9000, false)]
        [InlineData(10000, false)]
        public void GenExtension_IsLeapYear(int year, bool expected)
        {
            Assert.Equal(expected, year.IsLeapYear());
        }

        [Theory]
        [InlineData(0, 0, 0, true)]
        [InlineData(1900, 1, 1, true)]
        [InlineData(1898, 2, 31, false)]
        [InlineData(1890, 11, 31, false)]
        [InlineData(1980, 02, 29, true)]
        [InlineData(2000, 02, 29, true)]
        [InlineData(9999, 12, 31, true)]
        public void GenExtension_YearMonthDay_IsValidDate(int year, int month, int day, bool expected)
        {
            var datePart = new DatePart(year, month, day);

            Assert.Equal(expected, datePart.IsValid());
            Assert.IsType<DatePart>(datePart);
        }

        [Theory]
        [InlineData(0, 0, 0, false)]
        [InlineData(1900, 1, 1, true)]
        [InlineData(1898, 2, 31, false)]
        [InlineData(1890, 11, 31, false)]
        [InlineData(1980, 02, 29, true)]
        [InlineData(2000, 02, 29, true)]
        [InlineData(9999, 12, 31, true)]
        public void GenExtension_YearMonthDay_IsValidDateTime(int year, int month, int day, bool expected)
        {
            var datePart = new DatePart(year, month, day);

            Assert.Equal(expected, datePart.IsValidDateTime());
            Assert.IsType<DatePart>(datePart);
        }
    }
}
