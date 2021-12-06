using Xunit;

namespace GenDateTools.Test
{
    public class GenExtensionTests
    {

        [Theory]
        [InlineData(-20, "")]
        [InlineData(-1, "")]
        [InlineData(0, "")]
        [InlineData(1, "Jan")]
        [InlineData(2, "Feb")]
        [InlineData(3, "Mar")]
        [InlineData(4, "Apr")]
        [InlineData(5, "May")]
        [InlineData(6, "Jun")]
        [InlineData(7, "Jul")]
        [InlineData(8, "Aug")]
        [InlineData(9, "Sep")]
        [InlineData(10, "Oct")]
        [InlineData(11, "Nov")]
        [InlineData(12, "Dec")]
        [InlineData(13, "")]
        [InlineData(999999999, "")]
        public void GenExtension_ToMonthName_ReturnMonth(int month, string expected)
        {
            Assert.Equal(expected, month.ToMonthName());
        }

        [Theory]
        [InlineData("00000000", "00000000")]
        [InlineData("00000101", "00000101")]
        [InlineData("20000229", "20000229")]
        [InlineData("99991231", "99991231")]
        public void GenExtension_ToSortString_ReturnSortString(string datePartStr, string expected)
        {
            DatePart datePart = new DatePart(datePartStr);

            Assert.Equal(expected, datePart.ToSortString());
        }

        [Theory]
        [InlineData("00000000", "0000-00-00")]
        [InlineData("00000101", "0000-01-01")]
        [InlineData("20000229", "2000-02-29")]
        [InlineData("99991231", "9999-12-31")]
        public void GenExtension_ToIsoString_ReturnIsoString(string datePartStr, string expected)
        {
            DatePart datePart = new DatePart(datePartStr);

            Assert.Equal(expected, datePart.ToIsoString());
        }

        [Theory]
        [InlineData("00000000", "")]
        [InlineData("00000101", "01 Jan")]
        [InlineData("20000229", "29 Feb 2000")]
        [InlineData("99991231", "31 Dec 9999")]
        public void GenExtension_DatePartToGenString_ReturnGenString(string datePartStr, string expected)
        {
            DatePart datePart = new DatePart(datePartStr);

            Assert.Equal(expected, datePart.ToGenString());
        }

        [Theory]
        [InlineData("110000000310000000", "Abt. 1000")]
        [InlineData("119000101219000101", "01 Jan 1900")]
        [InlineData("118981101918981101", "Aft. 01 Nov 1898")]
        [InlineData("118901200618910300", "Bet. Dec 1890 - Mar 1891")]
        [InlineData("120180229720180301", "From 29 Feb 2018 to 01 Mar 2018")]
        [InlineData("199991231299991231", "31 Dec 9999")]
        [InlineData("100000031600000031", "Bet. 31 - 31")]
        [InlineData("100000431100000431", "Bef. 31 Apr")]
        [InlineData("100001000300001000", "Abt. Oct")]
        [InlineData("120170015420170015", "Cal. 15 2017")]
        [InlineData("100000001100000001", "Bef. 01")]
        [InlineData("100500000600700000", "Bet. 50 - 70")]
        [InlineData("108600000609100000", "Bet. 860 - 910")]
        public void GenExtension_GenDateToGenString_ReturnGenString(string genDateStr, string expected)
        {
            GenDate genDate = new GenDate(genDateStr);

            Assert.Equal(expected, genDate.ToGenString());
        }

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
            DatePart datePart = new DatePart(year, month, day);

            Assert.Equal(expected, datePart.IsValidDate());
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
            DatePart datePart = new DatePart(year, month, day);

            Assert.Equal(expected, datePart.IsValidDateTime());
            Assert.IsType<DatePart>(datePart);
        }
    }
}
