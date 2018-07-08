using Xunit;

namespace GenDate.Test
{
    public class DateSpanTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(365, 1, 0)]
        [InlineData(1000, 2, 270)]
        [InlineData(3651, 10, 1)]
        [InlineData(29199, 79, 364)]
        [InlineData(9999999, 27397, 94)]
        public void DataSpan_NewFromDays_ReturnsDateSpan(int expectedTotalDays, int expectedYears, int expectedDays)
        {
            var dateSpan = new DateSpan(expectedTotalDays);

            Assert.Equal(expectedYears, dateSpan.Years);
            Assert.Equal(expectedDays, dateSpan.Days);
            Assert.Equal(expectedTotalDays, dateSpan.TotalDays);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(365, 1, 0)]
        [InlineData(1000, 2, 270)]
        [InlineData(3651, 10, 1)]
        [InlineData(29199, 79, 364)]
        [InlineData(9999999, 27397, 94)]
        public void DataSpan_NewFromYearsAndDays_ReturnsDateSpan(int expectedTotalDays, int expectedYears, int expectedDays)
        {
            var dateSpan = new DateSpan(expectedYears, expectedDays);

            Assert.Equal(expectedYears, dateSpan.Years);
            Assert.Equal(expectedDays, dateSpan.Days);
            Assert.Equal(expectedTotalDays, dateSpan.TotalDays);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(0, 1231, 365, 1, 0)]
        [InlineData(0, 100000, 3652, 10, 0)]
        [InlineData(20000101, 20180102, 6576, 18, 1)]
        [InlineData(20001230, 20010102, 3, 0, 3)]
        [InlineData(20001230, 20030102, 733, 2, 3)]
        [InlineData(20001230, 20040102, 1098, 3, 3)]
        [InlineData(20001230, 20040302, 1158, 3, 63)]
        [InlineData(20001001, 20120301, 4169, 11, 152)]
        [InlineData(20180102, 20000101, 6576, 18, 1)]
        [InlineData(20010102, 20001230, 3, 0, 3)]
        [InlineData(20030102, 20001230, 733, 2, 3)]
        [InlineData(20040102, 20001230, 1098, 3, 3)]
        [InlineData(20040302, 20001230, 1158, 3, 63)]
        [InlineData(20120301, 20001001, 4169, 11, 152)]
        public void DataSpan_NewFromDateParts_ReturnsDateSpan(long datePartLong1, long datePartLong2, int expectedTotalDays, int expectedYears, int expectedDays)
        {
            var datePart1 = new DatePart(datePartLong1);
            var datePart2 = new DatePart(datePartLong2);

            var dateSpan = new DateSpan(datePart1, datePart2);

            Assert.Equal(expectedTotalDays, dateSpan.TotalDays);
            Assert.Equal(expectedYears, dateSpan.Years);
            Assert.Equal(expectedDays, dateSpan.Days);
        }

        [Theory]
        [InlineData(0, "0 days")]
        [InlineData(1, "1 day")]
        [InlineData(2, "2 days")]
        [InlineData(100, "100 days")]
        [InlineData(365, "1 year")]
        [InlineData(366, "1 year and 1 day")]
        [InlineData(367, "1 year and 2 days")]
        [InlineData(730, "2 years")]
        [InlineData(800, "2 years and 70 days")]
        [InlineData(3650, "10 years")]
        public void DataSpan_ToString_ReturnsValidString(int totalDays, string expected)
        {
            var dateSpan = new DateSpan(totalDays);

            Assert.Equal(expected, dateSpan.ToString());
        }

    }
}
