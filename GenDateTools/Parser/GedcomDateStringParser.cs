using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GenDateTools.Parser
{
    public class GedcomDateStringParser : DateStringParser
    {
        private static readonly Dictionary<string, int> rangeTypePre = new Dictionary<string, int>() { { "BET", 6 }, { "FROM", 7 } };
        private static readonly Dictionary<string, int> rangeTypeMid = new Dictionary<string, int>() { { "AND", 6 }, { "TO", 7 } };
        private static readonly Dictionary<string, int> modifierType = new Dictionary<string, int>() { { "BEF", 1 }, { "ABT", 3 }, { "CAL", 4 }, { "EST", 5 }, { "INT", 8 }, { "AFT", 9 } };
        private static readonly Dictionary<string, int> modTextType = new Dictionary<string, int>() { { "INT", 8 } };

        private static readonly Regex regexRange = new Regex(@"^(?<rangepre>"
                                                             + string.Join("|", rangeTypePre.Keys) + @")\s+?(?<fromdate>.+?)\s+?(?<rangemid>"
                                                             + string.Join("|", rangeTypeMid.Keys) + @")\s+?(?<todate>.+)");
        private static readonly Regex regexModText = new Regex(@"^(?<mod>" + string.Join("|", modTextType.Keys) + @")\s+?(?<date>[^(]+)[\s]+\((?<text>.+)\)", RegexOptions.IgnoreCase);
        private static readonly Regex regexModifier = new Regex(@"^(?<mod>" + string.Join("|", modifierType.Keys) + @")\s+?(?<date>[\w\d]{1,4}\b.*)");
        private static readonly Regex regexPhrase = new Regex(@"^\((?<phrase>.*?)\)");

        private static readonly string beginRegex = "^";
        private static readonly string endRegex = "$";
        private static readonly string spaceRegex = @"[\s]+";
        private static readonly string dayRegex = @"(?<day>\d{1,2})";
        private static readonly string monthRegex = @"(?<month>" + string.Join("|", GenTools.MonthsFromName().Keys) + ")";
        private static readonly string yearRegex = @"(?<year>\d{1,4})";

        private static readonly Regex regexDayMonYear = new Regex(beginRegex + dayRegex + spaceRegex + monthRegex + spaceRegex + yearRegex + endRegex);
        private static readonly Regex regexMonYear = new Regex(beginRegex + monthRegex + spaceRegex + yearRegex + endRegex);
        private static readonly Regex regexYear = new Regex(beginRegex + yearRegex + endRegex);

        public override GenDate Parse(string dateString)
        {
            var dateStringUpper = dateString.ToUpper();

            // Try to match a range
            var rMatch = regexRange.Match(dateStringUpper);
            if (rMatch.Success)
            {
                var rangePreVal = rMatch.Groups["rangepre"].Value;
                var rangeMidVal = rMatch.Groups["rangemid"].Value;

                if (rangeTypePre.ContainsKey(rangePreVal) 
                    && rangeTypeMid.ContainsKey(rangeMidVal)
                    && rangeTypePre[rangePreVal].ToString() == rangeTypeMid[rangeMidVal].ToString()
                    && Enum.TryParse(rangeTypePre[rangePreVal].ToString(), out GenDateType dateType))
                {
                    var fromDate = GetDatePartFromStringDate(rMatch.Groups["fromdate"].Value);
                    var toDate = GetDatePartFromStringDate(rMatch.Groups["todate"].Value);

                    return new GenDate(dateType, fromDate, toDate, fromDate.IsValidDate() && toDate.IsValidDate());
                }
            }

            // Try to match date with a modifier and a text
            var mTextMatch = regexModText.Match(dateString);
            if (mTextMatch.Success)
            {
                var modVal = mTextMatch.Groups["mod"].Value.ToUpper();

                if (modTextType.ContainsKey(modVal) && Enum.TryParse(modTextType[modVal].ToString().ToUpper(), out GenDateType dateType))
                {
                    var date = GetDatePartFromStringDate(mTextMatch.Groups["date"].Value.ToUpper());
                    var datePhrase = mTextMatch.Groups["text"].Value != null ? mTextMatch.Groups["text"].Value : string.Empty;

                    return new GenDate(dateType, date, date, datePhrase, date.IsValidDate());
                }
            }

            // Try to match date with a modifier
            var mMatch = regexModifier.Match(dateStringUpper);
            if (mMatch.Success)
            {
                var modVal = mMatch.Groups["mod"].Value;

                if (modifierType.ContainsKey(modVal) && Enum.TryParse(modifierType[modVal].ToString(), out GenDateType dateType))
                {
                    var date = GetDatePartFromStringDate(mMatch.Groups["date"].Value);

                    return new GenDate(dateType, date, date, date.IsValidDate());
                }
            }

            // A regular date
            var exactDate = GetDatePartFromStringDate(dateStringUpper);
            if (exactDate != DatePart.MinValue)
                return new GenDate(GenDateType.Exact, exactDate, exactDate, exactDate.IsValidDate());

            // Try to match a phrase
            var pMatch = regexPhrase.Match(dateString);
            if (pMatch.Success)
            {
                var datePhrase = pMatch.Groups["phrase"].Value;

                return new GenDate(GenDateType.Invalid, datePhrase, false);
            }

            // Last resort - return an empty GenDate
            return new GenDate(GenDateType.Invalid, string.Empty, false);
        }

        public override DatePart GetDatePartFromStringDate(string sDate)
        {
            // Match on day month year
            var mDayMonYear = regexDayMonYear.Match(sDate);
            if (mDayMonYear.Success && GenTools.MonthsFromName().ContainsKey(mDayMonYear.Groups["month"].Value))
                return new DatePart(mDayMonYear.Groups["year"].Value, GenTools.MonthsFromName()[mDayMonYear.Groups["month"].Value].ToString(), mDayMonYear.Groups["day"].Value);

            // Match on month year
            var mMonYear = regexMonYear.Match(sDate);
            if (mMonYear.Success && GenTools.MonthsFromName().ContainsKey(mMonYear.Groups["month"].Value))
                return new DatePart(mMonYear.Groups["year"].Value, GenTools.MonthsFromName()[mMonYear.Groups["month"].Value].ToString(), "0");

            // Match on year
            var mYear = regexYear.Match(sDate);
            if (mYear.Success)
                return new DatePart(mYear.Groups["year"].Value, "0", "0");

            return new DatePart();
        }
    }
}
