using GenDateTools.Models;
using Xunit;

namespace GenDateTools.Test
{
    public class GenDateTests
    {
        [Theory]
        [InlineData(119000101219000101, "119000101219000101")]
        [InlineData(118981101619000101, "118981101619000101")]
        [InlineData(118900101719000101, "118900101719000101")]
        [InlineData(120160229120160229, "120160229120160229")]
        [InlineData(199991231999991231, "199991231999991231")]
        [InlineData(100000000300000000, "100000000300000000")]
        public void GenDate_NewFromLong_ValidGenDate(long dateNum, string expected)
        {
            GenDate genDate = new GenDate(dateNum);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData("119000101219000101", "119000101219000101")]
        [InlineData("118981101619000101", "118981101619000101")]
        [InlineData("118900101719000101", "118900101719000101")]
        [InlineData("120160229120160229", "120160229120160229")]
        [InlineData("199991231999991231", "199991231999991231")]
        [InlineData("100000000300000000", "100000000300000000")]
        public void GenDate_NewFromString_ValidGenDate(string dateString, string expected)
        {
            GenDate genDate = new GenDate(dateString);

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
            DatePart datePart = new DatePart(datePartString);
            GenDate genDate = new GenDate(datePart);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData(1, "19000101", "119000101119000101")]
        [InlineData(3, "99990022", "199990022399990022")]
        [InlineData(9, "00000000", "100000000900000000")]
        [InlineData(1, "00000001", "100000001100000001")]
        [InlineData(9, "99991231", "199991231999991231")]
        [InlineData(4, "20000229", "120000229420000229")]
        public void GenDate_NewFromDateTypeAndDatePart_ValidGenDate(int dateType, string datePartString, string expected)
        {
            DatePart datePart = new DatePart(datePartString);
            GenDate genDate = new GenDate((GenDateType)dateType, datePart);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData(1, "19000101", "19000101", "119000101119000101")]
        [InlineData(6, "20171231", "20180123", "120171231620180123")]
        [InlineData(7, "20171231", "20180301", "120171231720180301")]
        [InlineData(3, "99990022", "99990022", "199990022399990022")]
        [InlineData(9, "00000000", "00000000", "100000000900000000")]
        [InlineData(9, "99991231", "99991231", "199991231999991231")]
        public void GenDate_NewFromDateTypeFromDatePartAndToDatePart_ValidGenDate(int dateType, string fromDatePartStr, string toDatePartStr, string expected)
        {
            DatePart fromDatePart = new DatePart(fromDatePartStr);
            DatePart toDatePart = new DatePart(toDatePartStr);
            GenDate genDate = new GenDate((GenDateType)dateType, fromDatePart, toDatePart);

            Assert.Equal(expected, genDate.DateString);
        }

        [Theory]
        [InlineData(1, "19000101", "19000101", true, "119000101119000101")]
        [InlineData(6, "20171231", "20180123", true, "120171231620180123")]
        [InlineData(7, "20171231", "20180301", true, "120171231720180301")]
        [InlineData(3, "99990022", "99990022", true, "199990022399990022")]
        [InlineData(9, "00000000", "00000000", true, "100000000900000000")]
        [InlineData(9, "99991231", "99991231", true, "199991231999991231")]
        public void GenDate_NewFromDateTypeFromDatePartAndToDatePartIsValid_ValidGenDate(int dateType, string fromDatePartStr, string toDatePartStr, bool isValid, string expected)
        {
            DatePart fromDatePart = new DatePart(fromDatePartStr);
            DatePart toDatePart = new DatePart(toDatePartStr);

            GenDate genDate = new GenDate((GenDateType)dateType, fromDatePart, toDatePart, isValid);
            GenDate expectedGenDate = new GenDate(expected);

            Assert.Equal(expectedGenDate, genDate);
        }

        [Theory]
        [InlineData(1, "19000101", "19000101", true, "119000101119000101")]
        [InlineData(6, "20171231", "20180123", true, "120171231620180123")]
        [InlineData(7, "20171231", "20180301", true, "120171231720180301")]
        [InlineData(3, "99990022", "99990022", true, "199990022399990022")]
        [InlineData(9, "00000000", "00000000", true, "100000000900000000")]
        [InlineData(9, "99991231", "99991231", true, "199991231999991231")]
        public void GenDate_NewFromAll_ValidGenDate(int dateType, string fromDatePartStr, string toDatePartStr, bool isValid, string expected)
        {
            DatePart fromDatePart = new DatePart(fromDatePartStr);
            DatePart toDatePart = new DatePart(toDatePartStr);
            string datePhrase = "Testing a pretty long string";

            GenDate genDate = new GenDate((GenDateType)dateType, fromDatePart, toDatePart, datePhrase, isValid);
            GenDate expectedGenDate = new GenDate(expected);

            Assert.Equal(expectedGenDate.DateLong, genDate.DateLong);
            Assert.Equal(expectedGenDate.DateString, genDate.DateString);
            Assert.Equal(expectedGenDate.ToString(), genDate.ToString());
            Assert.Equal(datePhrase, genDate.DatePhrase);
            Assert.True(genDate.IsValid);
        }

        [Fact]
        public void GenDate_CallWithToDatePartEqualNull_ShouldReturnValid()
        {
            string datePhrase = "Testing a pretty long string";
            GenDate genDate = new GenDate(GenDateType.Before, new DatePart("19000101"), null, datePhrase, true);
            GenDate expected = new GenDate("119000101119000101");

            Assert.Equal(expected.DateLong, genDate.DateLong);
            Assert.Equal(expected.DateString, genDate.DateString);
            Assert.Equal(expected.ToString(), genDate.ToString());
            Assert.Equal(datePhrase, genDate.DatePhrase);
            Assert.True(genDate.IsValid);
        }

        [Fact]
        public void GenDate_CallWithIsValidEqualFalse_ShouldReturnValid()
        {
            GenDate genDate = new GenDate(GenDateType.Before, new DatePart("19000101"), new DatePart("19000101"), "", false);
            GenDate expected = new GenDate("119000101119000101");

            Assert.Equal(expected.DateLong, genDate.DateLong);
            Assert.Equal(expected.DateString, genDate.DateString);
            Assert.True(genDate.DateFrom.IsValidDate());
            Assert.True(genDate.DateTo.IsValidDate());
        }

        [Theory]
        [InlineData(119000101219000101)]
        [InlineData(118981101619000101)]
        [InlineData(118900101719000101)]
        [InlineData(120160229120160229)]
        [InlineData(110000000310000000)]
        [InlineData(100000031600000031)]
        [InlineData(100000000900000000)]
        [InlineData(199991231999991231)]
        public void GenDate_GetDateLong_ValidGenDate(long dateNum)
        {
            GenDate genDate = new GenDate(dateNum);

            Assert.Equal(dateNum, genDate.DateLong);
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
        public void Gendate_ToString_EqualString(string dateString, string expected)
        {
            GenDate genDate = new GenDate(dateString);

            Assert.Equal(expected, genDate.ToString());
        }

        [Theory]
        [InlineData(119000101219000101, "119000101219000101")]
        [InlineData(118981101619000101, "118981101619000101")]
        [InlineData(118900101719000101, "118900101719000101")]
        [InlineData(120180229120180229, "120180229120180229")]
        [InlineData(199991231999991231, "199991231999991231")]
        [InlineData(100000000300000000, "100000000300000000")]
        public void GenDate_Equals_ReturnsTrue(long dateNum, string compareDate)
        {
            GenDate genDate1 = new GenDate(dateNum);
            GenDate genDate2 = new GenDate(compareDate);

            Assert.True(genDate1.Equals(genDate2));
        }

        [Theory]
        [InlineData(119000101219000101, "119000101219000101")]
        [InlineData(118981101519000101, "118981101519000101")]
        [InlineData(118900101619000101, "118900101619000101")]
        [InlineData(120180229120180229, "120180229120180229")]
        [InlineData(199991231799991231, "199991231799991231")]
        [InlineData(100000000300000000, "100000000300000000")]
        public void GenDate_OperatorEqual_ReturnsTrue(long dateNum, string compareDate)
        {
            GenDate genDate1 = new GenDate(dateNum);
            GenDate genDate2 = new GenDate(compareDate);

            Assert.True(genDate1 == genDate2);
        }

        [Theory]
        [InlineData(119000101219000101, "119000101219000101")]
        [InlineData(118981101519000101, "118981101519000101")]
        [InlineData(118900101619000101, "118900101619000101")]
        [InlineData(120180229120180229, "120180229120180229")]
        [InlineData(199991231799991231, "199991231799991231")]
        [InlineData(100000000300000000, "100000000300000000")]
        public void GenDate_OperatorNotEqual_ReturnsTrue(long dateNum, string compareDate)
        {
            GenDate genDate1 = new GenDate(dateNum);
            GenDate genDate2 = new GenDate(compareDate);

            Assert.False(genDate1 != genDate2);
        }

        [Theory]
        [InlineData(119000121219000121, "119000101219000101")]
        [InlineData(118981102519000110, "118981102519000101")]
        [InlineData(118900106619000107, "118900106619000106")]
        [InlineData(120180329120180329, "120180229120180229")]
        [InlineData(199991231799991231, "199991231399991231")]
        [InlineData(100000000300000000, "100000000200000000")]
        public void GenDate_OperatorLargerThan_ReturnsTrue(long dateNum, string compareDate)
        {
            GenDate genDate1 = new GenDate(dateNum);
            GenDate genDate2 = new GenDate(compareDate);

            Assert.True(genDate1 > genDate2);
        }

        [Theory]
        [InlineData(119000101219000101, "119000121219000121")]
        [InlineData(118981101519000101, "118981102519000101")]
        [InlineData(118900105619000106, "118900106619000107")]
        [InlineData(120180229120180229, "120180329120180329")]
        [InlineData(199991231399991231, "199991231799991231")]
        [InlineData(100000000200000000, "100000000300000000")]
        public void GenDate_OperatorLessThan_ReturnsTrue(long dateNum, string compareDate)
        {
            GenDate genDate1 = new GenDate(dateNum);
            GenDate genDate2 = new GenDate(compareDate);

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
            GenDate genDate1 = new GenDate(dateNum);
            GenDate genDate2 = new GenDate(compareDate);

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
            GenDate genDate1 = new GenDate(dateNum);
            GenDate genDate2 = new GenDate(compareDate);

            Assert.True(genDate1 <= genDate2);
        }

        [Theory]
        [InlineData(119000101219000101, 19000101)]
        [InlineData(119000101119000101, 18800101)]
        [InlineData(120000301120000301, 19800301)]
        [InlineData(119000101919000101, 19000102)]
        [InlineData(120000301920000301, 20000302)]
        [InlineData(120000301420000301, 19990301)]
        [InlineData(120000301520000301, 19990301)]
        [InlineData(119000101619001200, 19000101)]
        [InlineData(119000101719001200, 19000101)]
        public void GenDate_From_ReturnsInt(long dateNum, int expected)
        {
            GenDate genDate = new GenDate(dateNum);

            Assert.Equal(expected, genDate.From);
        }

        [Theory]
        [InlineData(119000101219000101, 19000101)]
        [InlineData(119000101119000101, 18991231)]
        [InlineData(120000301120000301, 20000229)]
        [InlineData(119000101919000101, 19200101)]
        [InlineData(120000301920000301, 20200301)]
        [InlineData(120000301420000301, 20010301)]
        [InlineData(120000301520000301, 20010301)]
        [InlineData(119000101619001200, 19001231)]
        [InlineData(119000101719001200, 19001231)]
        public void GenDate_To_ReturnsInt(long dateNum, int expected)
        {
            GenDate genDate = new GenDate(dateNum);

            Assert.Equal(expected, genDate.To);
        }

        [Fact]
        public void GenDate_GetDateRangeStrategy_ReturnsDefault()
        {
            DateRangeStrategy actual = DateRangeStrategy.Strategy;

            Assert.IsAssignableFrom<DateRangeStrategy>(actual);
        }

        [Fact]
        public void GenDate_SetDateRangeStrategy_ReturnsChanged()
        {
            DateRangeStrategy expected = new DateRangeStrategy
            {
                AfterYears = 10,
                BeforeYears = 10,
                AboutYearsAfter = 1,
                AboutYearsBefore = 1,
                UseRelaxedDates = false,
            };
            DateRangeStrategy.Strategy = expected;

            DateRangeStrategy actual = DateRangeStrategy.Strategy;

            Assert.Equal(expected.AboutYearsAfter, actual.AboutYearsAfter);
            Assert.Equal(expected.AboutYearsBefore, actual.AboutYearsBefore);
            Assert.Equal(expected.AfterYears, actual.AfterYears);
            Assert.Equal(expected.BeforeYears, actual.BeforeYears);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(20200220, 20200200)]
        [InlineData(20200000, 20200000)]
        [InlineData(99991231, 99991200)]
        public void GenDate_From_DateRangeStrategyUseRelaxedDates_ReturnsZeroDays(int input, int expected)
        {
            GenDate genDate = new GenDate(new DatePart(input))
            {
                DateRangeStrategy = new DateRangeStrategy
                {
                    AfterYears = 10,
                    BeforeYears = 10,
                    AboutYearsAfter = 0,
                    AboutYearsBefore = 0,
                    UseRelaxedDates = true,
                }
            };

            Assert.Equal(expected, genDate.From);
        }

        [Theory]
        [InlineData(10000100, 10000200)]
        [InlineData(20200220, 20200300)]
        [InlineData(20000131, 20000200)]
        [InlineData(99991130, 99991200)]
        public void GenDate_To_DateRangeStrategyUseRelaxedDates_ReturnsNextMonthZeroDays(int input, int expected)
        {
            GenDate genDate = new GenDate(new DatePart(input))
            {
                DateRangeStrategy = new DateRangeStrategy
                {
                    AfterYears = 10,
                    BeforeYears = 10,
                    AboutYearsAfter = 0,
                    AboutYearsBefore = 0,
                    UseRelaxedDates = true,
                }
            };

            Assert.Equal(expected, genDate.To);
        }

        [Theory]
        [InlineData(10000100, 9990100)]
        [InlineData(20200220, 20190220)]
        [InlineData(20040229, 20030229)]
        public void GenDate_From_DateRangeStrategyAboutYears_ReturnsYearBefore(int input, int expected)
        {
            GenDate genDate = new GenDate(GenDateType.About, new DatePart(input))
            {
                DateRangeStrategy = new DateRangeStrategy
                {
                    AfterYears = 0,
                    BeforeYears = 0,
                    AboutYearsAfter = 0,
                    AboutYearsBefore = 1,
                    UseRelaxedDates = false,
                }
            };

            Assert.Equal(expected, genDate.From);
        }

        [Theory]
        [InlineData(10000100, 10010131)]
        [InlineData(20200220, 20210220)]
        [InlineData(20000131, 20010131)]
        [InlineData(99981231, 99991231)]
        public void GenDate_To_DateRangeStrategyAboutYears_ReturnsYearAfter(int input, int expected)
        {
            GenDate genDate = new GenDate(GenDateType.About, new DatePart(input))
            {
                DateRangeStrategy = new DateRangeStrategy
                {
                    AfterYears = 0,
                    BeforeYears = 0,
                    AboutYearsAfter = 1,
                    AboutYearsBefore = 0,
                    UseRelaxedDates = false,
                }
            };

            Assert.Equal(expected, genDate.To);
        }
    }
}
