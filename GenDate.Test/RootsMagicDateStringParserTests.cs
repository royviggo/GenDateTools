using Xunit;

namespace GenDate.Test
{
    public class RootsMagicDateStringParserTests
    {
        [Theory]
        [InlineData("D.+17580000.A+00000000..", "117580000317580000")]
        [InlineData("D.+20110000.A+00000000..", "120110000320110000")]
        [InlineData("D.+17040000..+00000000..", "117040000217040000")]
        [InlineData("D.+00000610..+00000000..", "100000610200000610")]
        [InlineData("D.+20000229..+00000000..", "120000229220000229")]
        [InlineData("DB+17270000..+00000000..", "117270000117270000")]
        [InlineData("DB+17900000..+00000000..", "117900000117900000")]
        [InlineData("DB+17310923..+00000000..", "117310923117310923")]
        [InlineData("DR+19350000..+19930000..", "119350000519930000")]
        [InlineData("DR+16571030..+16580411..", "116571030516580411")]
        [InlineData("DR+20000101..+20000131..", "120000101520000131")]
        [InlineData("DA+18010000..+00000000..", "118010000718010000")]
        [InlineData("DA+19200000..+00000000..", "119200000719200000")]
        [InlineData("DS+20000101..+20000131..", "120000101620000131")]
        [InlineData("D.+00010120.L+00000000..", "100010120400010120")]
        [InlineData("D.+20011105.L+00000000..", "120011105420011105")]
        public void RootsMagicDateStringParser_Parse_ReturnsValidGenDate(string dateString, string expected)
        {
            var parser = new RootsMagicDateStringParser();
            var genDate = new GenDate(parser, dateString);
            var expectedGenDate = new GenDate(expected);

            Assert.Equal(expectedGenDate, genDate);
        }

        [Theory]
        [InlineData("TOmkr about 1765", "Omkr about 1765")]
        [InlineData("TLorem ipsum dolor sit amet, ea elit ferri per, choro semper ne qui.", "Lorem ipsum dolor sit amet, ea elit ferri per, choro semper ne qui.")]
        [InlineData(".", "")]
        public void RootsMagicDateStringParser_Parse_ReturnsInvalidGenDate(string dateString, string expected)
        {
            var parser = new RootsMagicDateStringParser();
            var genDate = new GenDate(parser, dateString);

            Assert.Equal(expected, genDate.DatePhrase);
            Assert.Equal(expected, genDate.ToString());
            Assert.False(genDate.IsValid);
        }
    }
}
