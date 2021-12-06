using Xunit;

namespace GenDateTools.Test
{
    public class GetToolsTests
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
        public void GenTools_MonthName_ReturnMonth(int month, string expected)
        {
            Assert.Equal(expected, GenTools.MonthName(month));
        }
    }
}