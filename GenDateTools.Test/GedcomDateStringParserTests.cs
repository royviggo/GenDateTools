using GenDateTools.Parser;
using Xunit;

namespace GenDateTools.Test
{
    public class GedcomDateStringParserTests
    {
        [Theory]
        [InlineData("1 JAN 1900", "119000101219000101")]
        [InlineData("FEB 1900", "119000200219000200")]
        [InlineData("1900", "119000000219000000")]
        [InlineData("AFT 1900", "119000000919000000")]
        [InlineData("BEF 1 MAR 1900", "119000301119000301")]
        [InlineData("ABT 1 APR 1900", "119000401319000401")]
        [InlineData("CAL 1 MAY 1900", "119000501419000501")]
        [InlineData("EST 1 JUN 1900", "119000601519000601")]
        [InlineData("INT 1 JUL 1900", "119000701819000701")]
        [InlineData("AFT 1 AUG 1900", "119000801919000801")]
        [InlineData("BET 30 SEP 1900 AND 31 OCT 1900", "119000930619001031")]
        [InlineData("BET SEP 1900 AND OCT 1900", "119000900619001000")]
        [InlineData("FROM 10 NOV 1900 TO 31 DEC 1900", "119001110719001231")]
        [InlineData("FROM 1900 TO 1910", "119000000719100000")]
        [InlineData("(some text)", "2some text")]
        [InlineData("some text)", "2")]
        [InlineData("(some text", "2")]
        public void GedcomDateStringParser_Parse_ReturnsValidGenDate(string dateString, string expected)
        {
            GedcomDateStringParser parser = new GedcomDateStringParser();
            GenDate genDate = new GenDate(parser, dateString);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData("INT 1 JUL 1900 (some text)", "119000701819000701", "some text")]
        public void GedcomDateStringParser_ParseInterpretedWithText_ReturnsValidGenDate(string dateString, string expected, string expectedPhrase)
        {
            GedcomDateStringParser parser = new GedcomDateStringParser();
            GenDate genDate = new GenDate(parser, dateString);
            GenDate expectedGenDate = new GenDate(expected);

            Assert.Equal(expectedGenDate.DateString, genDate.DateString);
            Assert.Equal(expectedPhrase, genDate.DatePhrase);
        }

    }
}
