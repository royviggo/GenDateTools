using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GenDateTools.Parser
{
    public class GedcomDateStringParser : DateStringParser
    {
        public override GenDate Parse(string dateString)
        {
            var rangeTypePre = new Dictionary<string, int>() { { "BET", 6 }, { "FROM", 7 } };
            var rangeTypeMid = new Dictionary<string, int>() { { "AND", 6 }, { "TO", 7 } };
            var modifierType = new Dictionary<string, int>() { { "BEF", 1 }, { "ABT", 3 }, { "CAL", 4 }, { "EST", 5 }, { "INT", 8 }, { "AFT", 9 } };
            var modTextType = new Dictionary<string, int>() { { "INT", 8 } };

            var regexRange = new Regex(@"^(?<rangepre>" 
                                       + string.Join("|", rangeTypePre.Keys) + @")\s+?(?<fromdate>.+?)\s+?(?<rangemid>"
                                       + string.Join("|", rangeTypeMid.Keys) + @")\s+?(?<todate>.+)");
            var regexModText = new Regex(@"^(?<mod>" + string.Join("|", modTextType.Keys) + @")\s+?(?<date>[^(]+)[\s]+\((?<text>.+)\)", RegexOptions.IgnoreCase);
            var regexModifier = new Regex(@"^(?<mod>" + string.Join("|", modifierType.Keys) + @")\s+?(?<date>[\w\d]{1,4}\b.*)");
            var regexPhrase = new Regex(@"^\((?<phrase>.*?)\)");

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
                var modVal = mTextMatch.Groups["mod"].Value;

                if (modTextType.ContainsKey(modVal) && Enum.TryParse(modTextType[modVal].ToString(), out GenDateType dateType))
                {
                    var date = GetDatePartFromStringDate(mTextMatch.Groups["date"].Value);
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
            var months = GenTools.MonthsFromName();

            var beginRegex = "^";
            var endRegex = "$";
            var spaceRegex = @"[\s]+";
            var dayRegex = @"(?<day>\d{1,2})";
            var monthRegex = @"(?<month>" + string.Join("|", months.Keys) + ")";
            var yearRegex = @"(?<year>\d{1,4})";

            var regexDayMonYear = new Regex(beginRegex + dayRegex + spaceRegex + monthRegex + spaceRegex + yearRegex + endRegex);
            var regexMonYear = new Regex(beginRegex + monthRegex + spaceRegex + yearRegex + endRegex);
            var regexYear = new Regex(beginRegex + yearRegex + endRegex);

            // Match on day month year
            var mDayMonYear = regexDayMonYear.Match(sDate);
            if (mDayMonYear.Success && months.ContainsKey(mDayMonYear.Groups["month"].Value))
                return new DatePart(mDayMonYear.Groups["year"].Value, months[mDayMonYear.Groups["month"].Value].ToString(), mDayMonYear.Groups["day"].Value);

            // Match on month year
            var mMonYear = regexMonYear.Match(sDate);
            if (mMonYear.Success && months.ContainsKey(mMonYear.Groups["month"].Value))
                return new DatePart(mMonYear.Groups["year"].Value, months[mMonYear.Groups["month"].Value].ToString(), "0");

            // Match on year
            var mYear = regexYear.Match(sDate);
            if (mYear.Success)
                return new DatePart(mYear.Groups["year"].Value, "0", "0");

            return new DatePart();
        }
    }
}
