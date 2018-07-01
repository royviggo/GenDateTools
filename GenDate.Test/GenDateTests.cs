using Xunit;

namespace GenDate.Test
{
    public class GenDateTests
    {
        [Theory]
        [InlineData(119000101219000101, "119000101219000101")]
        [InlineData(118981101519000101, "118981101519000101")]
        [InlineData(118900101619000101, "118900101619000101")]
        [InlineData(120180229120180229, "120180229120180229")]
        public void New_FromLong_ValidGenDate(long dateNum, string expected)
        {
            var genDate = new GenDate(dateNum);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData("119000101219000101", "119000101219000101")]
        [InlineData("118981101519000101", "118981101519000101")]
        [InlineData("118900101619000101", "118900101619000101")]
        [InlineData("120180229120180229", "120180229120180229")]
        public void New_FromString_ValidGenDate(string dateString, string expected)
        {
            var genDate = new GenDate(dateString);

            Assert.Equal(expected, genDate.DateString);
        }
    }
}
