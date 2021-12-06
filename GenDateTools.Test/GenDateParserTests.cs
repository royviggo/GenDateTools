using GenDateTools.Parser;
using Xunit;

namespace GenDateTools.Test
{
    public class GenDateParserTests
    {
        [Theory]
        [InlineData("1 Jan 1900", "119000101219000101")]
        [InlineData("Feb 1900", "119000200219000200")]
        [InlineData("1900", "119000000219000000")]
        [InlineData("Aft. 1900", "119000000919000000")]
        [InlineData("Bef. 1 Mar 1900", "119000301119000301")]
        [InlineData("Abt. 1 Apr 1900", "119000401319000401")]
        [InlineData("Cal. 1 May 1900", "119000501419000501")]
        [InlineData("Est. 1 Jun 1900", "119000601519000601")]
        [InlineData("Int. 1 Jul 1900", "119000701819000701")]
        [InlineData("Aft. 1 Aug 1900", "119000801919000801")]
        [InlineData("Bet. 30 Sep 1900 - 31 Oct 1900", "119000930619001031")]
        [InlineData("Bet. Sep 1900 - Oct 1900", "119000900619001000")]
        [InlineData("From 10 Nov 1900 to 31 Dec 1900", "119001110719001231")]
        [InlineData("From 1900 to 1910", "119000000719100000")]
        [InlineData("(some text)", "2some text")]
        [InlineData("some text)", "2")]
        [InlineData("(some text", "2")]
        public void GenDateParser_Parse_ReturnsValidGenDate(string dateString, string expected)
        {
            GenDateParser parser = new GenDateParser();
            GenDate genDate = new GenDate(parser, dateString);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData("Int. 1 Jul 1900 (some text)", "119000701819000701", "some text")]
        public void GenDateParser_ParseInterpretedWithText_ReturnsValidGenDate(string dateString, string expected, string expectedPhrase)
        {
            GenDateParser parser = new GenDateParser();
            GenDate genDate = new GenDate(parser, dateString);
            GenDate expectedGenDate = new GenDate(expected);

            Assert.Equal(expectedGenDate.DateString, genDate.DateString);
            Assert.Equal(expectedPhrase, genDate.DatePhrase);
        }
    }
}
