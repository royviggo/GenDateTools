using System;
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
        [InlineData(199991231799991231, "199991231799991231")]
        [InlineData(100000000300000000, "100000000300000000")]
        public void GenDate_NewFromLong_ValidGenDate(long dateNum, string expected)
        {
            var genDate = new GenDate(dateNum);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData("119000101219000101", "119000101219000101")]
        [InlineData("118981101519000101", "118981101519000101")]
        [InlineData("118900101619000101", "118900101619000101")]
        [InlineData("120180229120180229", "120180229120180229")]
        [InlineData("199991231799991231", "199991231799991231")]
        [InlineData("100000000300000000", "100000000300000000")]
        public void GenDate_NewFromString_ValidGenDate(string dateString, string expected)
        {
            var genDate = new GenDate(dateString);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData("19000101", "119000101219000101")]
        [InlineData("00000000", "100000000200000000")]
        [InlineData("00000001", "100000001200000001")]
        [InlineData("99990022", "199990022299990022")]
        [InlineData("99991231", "199991231299991231")]
        [InlineData("20000229", "120000229220000229")]
        public void GenDate_NewFromDatePart_ValidGenDate(string datePartString, string expected)
        {
            var datePart = new DatePart(datePartString);
            var genDate = new GenDate(datePart);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData(1, "19000101", "119000101119000101")]
        [InlineData(3, "99990022", "199990022399990022")]
        [InlineData(7, "00000000", "100000000700000000")]
        [InlineData(1, "00000001", "100000001100000001")]
        [InlineData(7, "99991231", "199991231799991231")]
        [InlineData(4, "20000229", "120000229420000229")]
        public void GenDate_NewFromDateTypeAndDatePart_ValidGenDate(int dateType, string datePartString, string expected)
        {
            var datePart = new DatePart(datePartString);
            var genDate = new GenDate((GenDateType)dateType, datePart);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData(1, "19000101", "19000101", "119000101119000101")]
        [InlineData(5, "20171231", "20180123", "120171231520180123")]
        [InlineData(6, "20171231", "20180301", "120171231620180301")]
        [InlineData(3, "99990022", "99990022", "199990022399990022")]
        [InlineData(7, "00000000", "00000000", "100000000700000000")]
        [InlineData(7, "99991231", "99991231", "199991231799991231")]
        public void GenDate_NewFromDateTypeFromDatePartAndToDatePart_ValidGenDate(int dateType, string fromDatePartStr, string toDatePartStr, string expected)
        {
            var fromDatePart = new DatePart(fromDatePartStr);
            var toDatePart = new DatePart(toDatePartStr);
            var genDate = new GenDate((GenDateType)dateType, fromDatePart, toDatePart);

            Assert.Equal(expected, genDate.DateString);
        }

        [Fact]
        public void GenDate_NewFromDateTypeFromDatePartAndToDatePartIsValid_ValidGenDate()
        {
            var fromDatePart = new DatePart(2018, 6, 23);
            var toDatePart = fromDatePart;
            var isValid = true;

            var genDate = new GenDate(GenDateType.Before, fromDatePart, toDatePart, isValid);

            Assert.Equal("20180623", genDate.DateFrom.ToSortString());
            Assert.Equal("20180623", genDate.DateTo.ToSortString());
            Assert.Equal(GenDateType.Before, genDate.DateType);
            Assert.Null(genDate.DatePhrase);
            Assert.True(genDate.IsValid);
        }

        [Fact]
        public void GenDate_NewFromAll_ValidGenDate()
        {
            var fromDatePart = new DatePart(2018, 6, 23);
            var toDatePart = fromDatePart;
            var datePhrase = "Testing testing";
            var isValid = true;

            var genDate = new GenDate(GenDateType.Before, fromDatePart, toDatePart, datePhrase, isValid);

            Assert.Equal("20180623", genDate.DateFrom.ToSortString());
            Assert.Equal("20180623", genDate.DateTo.ToSortString());
            Assert.Equal(GenDateType.Before, genDate.DateType);
            Assert.Equal(datePhrase, genDate.DatePhrase);
            Assert.True(genDate.IsValid);
        }

        [Theory]
        [InlineData(119000101219000101)]
        [InlineData(118981101519000101)]
        [InlineData(118900101619000101)]
        [InlineData(120180229120180229)]
        [InlineData(110000000310000000)]
        [InlineData(100000031500000031)]
        [InlineData(100000000700000000)]
        [InlineData(199991231799991231)]
        public void GenDate_GetDateLong_ValidGenDate(long dateNum)
        {
            var genDate = new GenDate(dateNum);

            Assert.Equal(dateNum, genDate.DateLong);
        }

        [Theory]
        [InlineData("110000000310000000", "Abt. 1000")]
        [InlineData("119000101219000101", "01 Jan 1900")]
        [InlineData("118981101718981101", "Aft. 01 Nov 1898")]
        [InlineData("118901200518910300", "Bet. Dec 1890 - Mar 1891")]
        [InlineData("120180229620180301", "From 29 Feb 2018 to 01 Mar 2018")]
        [InlineData("199991231299991231", "31 Dec 9999")]
        [InlineData("100000031500000031", "Bet. 31 - 31")]
        [InlineData("100000431100000431", "Bef. 31 Apr")]
        [InlineData("100001000300001000", "Abt. Oct")]
        [InlineData("120170015420170015", "Cal. 15 2017")]
        [InlineData("100000001100000001", "Bef. 01")]
        [InlineData("100500000500700000", "Bet. 50 - 70")]
        [InlineData("108600000509100000", "Bet. 860 - 910")]
        public void Gendate_ToString_EqualString(string dateString, string expected)
        {
            var genDate = new GenDate(dateString);

            Assert.Equal(expected, genDate.ToString());
        }

        [Theory]
        [InlineData(119000101219000101, "119000101219000101")]
        [InlineData(118981101519000101, "118981101519000101")]
        [InlineData(118900101619000101, "118900101619000101")]
        [InlineData(120180229120180229, "120180229120180229")]
        public void GenDate_Equals_ReturnsTrue(long dateNum, string compareDate)
        {
            var genDate1 = new GenDate(dateNum);
            var genDate2 = new GenDate(compareDate);

            Assert.True(genDate1.Equals(genDate2));
        }

        [Theory]
        [InlineData(119000101219000101, "119000101219000101")]
        [InlineData(118981101519000101, "118981101519000101")]
        [InlineData(118900101619000101, "118900101619000101")]
        [InlineData(120180229120180229, "120180229120180229")]
        public void GenDate_OperatorEqual_ReturnsTrue(long dateNum, string compareDate)
        {
            var genDate1 = new GenDate(dateNum);
            var genDate2 = new GenDate(compareDate);

            Assert.True(genDate1 == genDate2);
        }

        [Theory]
        [InlineData(119000101219000101, "119000101219000101")]
        [InlineData(118981101519000101, "118981101519000101")]
        [InlineData(118900101619000101, "118900101619000101")]
        [InlineData(120180229120180229, "120180229120180229")]
        public void GenDate_OperatorNotEqual_ReturnsTrue(long dateNum, string compareDate)
        {
            var genDate1 = new GenDate(dateNum);
            var genDate2 = new GenDate(compareDate);

            Assert.False(genDate1 != genDate2);
        }

        [Theory]
        [InlineData(119000121219000121, "119000101219000101")]
        [InlineData(118981102519000110, "118981102519000101")]
        [InlineData(118900106619000107, "118900106619000106")]
        [InlineData(120180329120180329, "120180229120180229")]
        public void GenDate_OperatorLargerThan_ReturnsTrue(long dateNum, string compareDate)
        {
            var genDate1 = new GenDate(dateNum);
            var genDate2 = new GenDate(compareDate);

            Assert.True(genDate1 > genDate2);
        }

        [Theory]
        [InlineData(119000101219000101, "119000121219000121")]
        [InlineData(118981101519000101, "118981102519000101")]
        [InlineData(118900105619000106, "118900106619000107")]
        [InlineData(120180229120180229, "120180329120180329")]
        public void GenDate_OperatorLessThan_ReturnsTrue(long dateNum, string compareDate)
        {
            var genDate1 = new GenDate(dateNum);
            var genDate2 = new GenDate(compareDate);

            Assert.True(genDate1 < genDate2);
        }

        [Theory]
        [InlineData(119000121219000121, "119000101219000101")]
        [InlineData(118981102519000110, "118981102519000101")]
        [InlineData(118900106619000107, "118900106619000106")]
        [InlineData(120180329120180329, "120180229120180229")]
        [InlineData(120000301120000301, "120000229220000229")]
        [InlineData(120000229220000229, "120000229220000229")]
        public void GenDate_OperatorLargerThanOrEqual_ReturnsTrue(long dateNum, string compareDate)
        {
            var genDate1 = new GenDate(dateNum);
            var genDate2 = new GenDate(compareDate);

            Assert.True(genDate1 >= genDate2);
        }

        [Theory]
        [InlineData(119000101219000101, "119000121219000121")]
        [InlineData(118981101519000101, "118981102519000101")]
        [InlineData(118900105619000106, "118900106619000107")]
        [InlineData(120180229120180229, "120180329120180329")]
        [InlineData(120000229220000229, "120000301120000301")]
        [InlineData(120000229220000229, "120000229220000229")]
        public void GenDate_OperatorLessThanOrEqual_ReturnsTrue(long dateNum, string compareDate)
        {
            var genDate1 = new GenDate(dateNum);
            var genDate2 = new GenDate(compareDate);

            Assert.True(genDate1 <= genDate2);
        }

    }
}
