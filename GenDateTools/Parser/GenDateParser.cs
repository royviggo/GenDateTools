using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GenDateTools.Parser
{
    public class GenDateParser : GedcomDateStringParser
    {
        private static readonly Dictionary<string, int> _months = new Dictionary<string, int> { { "jan", 1 }, { "feb", 2 }, { "mar", 3 }, { "mär", 3 }, { "maa", 3 }, { "apr", 4 },
            { "mai", 5 }, { "may", 5 },  { "maj", 5 },  { "mei", 5 }, { "jun", 6 }, { "jul", 7 }, { "aug", 8 }, { "sep", 9 }, {"oct", 10 },
            {"okt", 10 }, {"nov", 11 }, {"des", 12 }, {"dec", 12 }, {"dez", 12 } };

        private const string _beginRx = "^";
        private const string _endRx = "$";
        private const string _sepSpaceRx = @"[\s]+";
        private const string _sepIsoRx = @"-";
        private const string _sepDotRx = @"\.";
        private const string _dayRx = @"(?<day>\d{1,2})";
        private const string _monthNumRx = @"(?<month>\d{1,2})";
        private static readonly string _monthRx = @"(?<month>" + string.Join("|", _months.Keys) + ")";
        private const string _yearRx = @"(?<year>\d{1,4})";

        private static readonly Regex _regexDayMonYear = new Regex(_beginRx + _dayRx + _sepSpaceRx + _monthRx + _sepSpaceRx + _yearRx + _endRx);
        private static readonly Regex _regexMonYear = new Regex(_beginRx + _monthRx + _sepSpaceRx + _yearRx + _endRx);
        private static readonly Regex _regexNor = new Regex(_beginRx + _dayRx + _sepDotRx + _monthNumRx + _sepDotRx + _yearRx + _endRx);
        private static readonly Regex _regexIso = new Regex(_beginRx + _yearRx + _sepIsoRx + _monthNumRx + _sepIsoRx + _dayRx + _endRx);
        private static readonly Regex _regexIsoMonYear = new Regex(_beginRx + _yearRx + _sepIsoRx + _monthNumRx + _endRx);
        private static readonly Regex _regexYear = new Regex(_beginRx + _yearRx + _endRx);

        public override GenDate Parse(string dateString)
        {
            dateString = Regex.Replace(dateString, @"\.", "");
            dateString = Regex.Replace(dateString, @"-", "AND");

            return base.Parse(dateString);
        }

        public override DatePart GetDatePartFromStringDate(string sDate)
        {
            sDate = sDate.ToLowerInvariant();

            // Match on day month year
            Match mDayMonYear = _regexDayMonYear.Match(sDate);
            if (mDayMonYear.Success && _months.ContainsKey(mDayMonYear.Groups["month"].Value))
            {
                return new DatePart(mDayMonYear.Groups["year"].Value, _months[mDayMonYear.Groups["month"].Value].ToString(), mDayMonYear.Groups["day"].Value);
            }

            // Match on month year
            Match mMonYear = _regexMonYear.Match(sDate);
            if (mMonYear.Success && _months.ContainsKey(mMonYear.Groups["month"].Value))
            {
                return new DatePart(mMonYear.Groups["year"].Value, _months[mMonYear.Groups["month"].Value].ToString(), "0");
            }

            // Match on dd.mm.yyyy
            Match mNor = _regexNor.Match(sDate);
            if (mNor.Success)
            {
                return new DatePart(mNor.Groups["year"].Value, mNor.Groups["month"].Value, mNor.Groups["day"].Value);
            }

            // Match on ISO
            Match mIso = _regexIso.Match(sDate);
            if (mIso.Success)
            {
                return new DatePart(mIso.Groups["year"].Value, mIso.Groups["month"].Value, mIso.Groups["day"].Value);
            }

            // Match on ISO year and month
            Match mIsoYm = _regexIsoMonYear.Match(sDate);
            if (mIsoYm.Success)
            {
                return new DatePart(mIsoYm.Groups["year"].Value, mIsoYm.Groups["month"].Value, "0");
            }

            // Match on year
            Match mYear = _regexYear.Match(sDate);
            
            return mYear.Success 
                ? new DatePart(mYear.Groups["year"].Value, "0", "0") 
                : new DatePart();
        }
    }
}
