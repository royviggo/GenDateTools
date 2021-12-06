using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GenDateTools.Parser
{
    public class GedcomDateStringParser : DateStringParser
    {
        private static readonly Dictionary<string, int> _rangeTypePre = new Dictionary<string, int>() { { "BET", 6 }, { "FROM", 7 } };
        private static readonly Dictionary<string, int> _rangeTypeMid = new Dictionary<string, int>() { { "AND", 6 }, { "TO", 7 } };
        private static readonly Dictionary<string, int> _modifierType = new Dictionary<string, int>() { { "BEF", 1 }, { "ABT", 3 }, { "CAL", 4 }, { "EST", 5 }, { "INT", 8 }, { "AFT", 9 } };
        private static readonly Dictionary<string, int> _modTextType = new Dictionary<string, int>() { { "INT", 8 } };

        private static readonly Regex _regexRange = new Regex(@"^(?<rangepre>"
                                                             + string.Join("|", _rangeTypePre.Keys) + @")\s+?(?<fromdate>.+?)\s+?(?<rangemid>"
                                                             + string.Join("|", _rangeTypeMid.Keys) + @")\s+?(?<todate>.+)");
        private static readonly Regex _regexModText = new Regex(@"^(?<mod>" + string.Join("|", _modTextType.Keys) + @")\s+?(?<date>[^(]+)[\s]+\((?<text>.+)\)", RegexOptions.IgnoreCase);
        private static readonly Regex _regexModifier = new Regex(@"^(?<mod>" + string.Join("|", _modifierType.Keys) + @")\s+?(?<date>[\w\d]{1,4}\b.*)");
        private static readonly Regex _regexPhrase = new Regex(@"^\((?<phrase>.*?)\)");

        private static readonly string _beginRegex = "^";
        private static readonly string _endRegex = "$";
        private static readonly string _spaceRegex = @"[\s]+";
        private static readonly string _dayRegex = @"(?<day>\d{1,2})";
        private static readonly string _monthRegex = @"(?<month>" + string.Join("|", GenTools.MonthsFromName().Keys) + ")";
        private static readonly string _yearRegex = @"(?<year>\d{1,4})";

        private static readonly Regex _regexDayMonYear = new Regex(_beginRegex + _dayRegex + _spaceRegex + _monthRegex + _spaceRegex + _yearRegex + _endRegex);
        private static readonly Regex _regexMonYear = new Regex(_beginRegex + _monthRegex + _spaceRegex + _yearRegex + _endRegex);
        private static readonly Regex _regexYear = new Regex(_beginRegex + _yearRegex + _endRegex);

        public override GenDate Parse(string dateString)
        {
            string dateStringUpper = dateString.ToUpperInvariant();

            // Try to match a range
            Match rMatch = _regexRange.Match(dateStringUpper);
            if (rMatch.Success)
            {
                string rangePreVal = rMatch.Groups["rangepre"].Value;
                string rangeMidVal = rMatch.Groups["rangemid"].Value;

                if (_rangeTypePre.ContainsKey(rangePreVal)
                    && _rangeTypeMid.ContainsKey(rangeMidVal)
                    && _rangeTypePre[rangePreVal].ToString() == _rangeTypeMid[rangeMidVal].ToString()
                    && Enum.TryParse(_rangeTypePre[rangePreVal].ToString(), out GenDateType dateType))
                {
                    DatePart fromDate = GetDatePartFromStringDate(rMatch.Groups["fromdate"].Value);
                    DatePart toDate = GetDatePartFromStringDate(rMatch.Groups["todate"].Value);

                    return new GenDate(dateType, fromDate, toDate, fromDate.IsValidDate() && toDate.IsValidDate());
                }
            }

            // Try to match date with a modifier and a text
            Match mTextMatch = _regexModText.Match(dateString);
            if (mTextMatch.Success)
            {
                string modVal = mTextMatch.Groups["mod"].Value.ToUpperInvariant();

                if (_modTextType.ContainsKey(modVal) && Enum.TryParse(_modTextType[modVal].ToString().ToUpperInvariant(), out GenDateType dateType))
                {
                    DatePart date = GetDatePartFromStringDate(mTextMatch.Groups["date"].Value.ToUpperInvariant());
                    string datePhrase = mTextMatch.Groups["text"].Value;

                    return new GenDate(dateType, date, date, datePhrase, date.IsValidDate());
                }
            }

            // Try to match date with a modifier
            Match mMatch = _regexModifier.Match(dateStringUpper);
            if (mMatch.Success)
            {
                string modVal = mMatch.Groups["mod"].Value;

                if (_modifierType.ContainsKey(modVal) && Enum.TryParse(_modifierType[modVal].ToString(), out GenDateType dateType))
                {
                    DatePart date = GetDatePartFromStringDate(mMatch.Groups["date"].Value);

                    return new GenDate(dateType, date, date, date.IsValidDate());
                }
            }

            // A regular date
            DatePart exactDate = GetDatePartFromStringDate(dateStringUpper);
            if (exactDate != DatePart.MinValue)
            {
                return new GenDate(GenDateType.Exact, exactDate, exactDate, exactDate.IsValidDate());
            }

            // Try to match a phrase
            Match pMatch = _regexPhrase.Match(dateString);
            if (pMatch.Success)
            {
                string datePhrase = pMatch.Groups["phrase"].Value;

                return new GenDate(GenDateType.Invalid, datePhrase, false);
            }

            // Last resort - return an empty GenDate
            return new GenDate(GenDateType.Invalid, string.Empty, false);
        }

        public override DatePart GetDatePartFromStringDate(string sDate)
        {
            // Match on day month year
            Match mDayMonYear = _regexDayMonYear.Match(sDate);
            if (mDayMonYear.Success && GenTools.MonthsFromName().ContainsKey(mDayMonYear.Groups["month"].Value))
            {
                return new DatePart(mDayMonYear.Groups["year"].Value, GenTools.MonthsFromName()[mDayMonYear.Groups["month"].Value].ToString(), mDayMonYear.Groups["day"].Value);
            }

            // Match on month year
            Match mMonYear = _regexMonYear.Match(sDate);
            if (mMonYear.Success && GenTools.MonthsFromName().ContainsKey(mMonYear.Groups["month"].Value))
            {
                return new DatePart(mMonYear.Groups["year"].Value, GenTools.MonthsFromName()[mMonYear.Groups["month"].Value].ToString(), "0");
            }

            // Match on year
            Match mYear = _regexYear.Match(sDate);
            if (mYear.Success)
            {
                return new DatePart(mYear.Groups["year"].Value, "0", "0");
            }

            return new DatePart();
        }
    }
}
