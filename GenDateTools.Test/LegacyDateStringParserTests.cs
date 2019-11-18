using GenDateTools.Parser;
using Xunit;

namespace GenDateTools.Test
{
    public class LegacyDateStringParserTests
    {
        [Theory]
        [InlineData("000101190000000000", "119000101219000101")]
        [InlineData("000002190000000000", "119000200219000200")]
        [InlineData("000000190000000000", "119000000219000000")]
        [InlineData("300103190000000000", "119000301119000301")]
        [InlineData("400000190000000000", "119000000919000000")]
        [InlineData("100104190000000000", "119000401319000401")]
        [InlineData("200104190000000000", "119000401319000401")]
        [InlineData("h00105190000000000", "119000501419000501")]
        [InlineData("M00000200500002006", "120050000720060000")]
        [InlineData("503009190031101900", "119000930619001031")]
        [InlineData("500009190000101900", "119000900619001000")]
        [InlineData("F01011190031121900", "119001110719001231")]
        [InlineData("F00000190000001910", "119000000719100000")]
        [InlineData(@":00000183100000000\Langfredag? 1831", "118310000218310000")]
        public void LegacyDateStringParser_Parse_ReturnsValidGenDate(string dateString, string expected)
        {
            var parser = new LegacyDateStringParser();
            var genDate = new GenDate(parser, dateString);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData(@":00000183100000000\Langfredag? 1831", "118310000218310000", "Langfredag? 1831")]
        public void LegacyDateStringParser_ParseIntepretedWithText_ReturnsValidGenDate(string dateString, string expected, string expectedPhrase)
        {
            var parser = new LegacyDateStringParser();
            var genDate = new GenDate(parser, dateString);
            var expectedGenDate = new GenDate(expected);

            Assert.Equal(expectedGenDate.DateString, genDate.DateString);
            Assert.Equal(expectedPhrase, genDate.DatePhrase);
        }
    }
}
