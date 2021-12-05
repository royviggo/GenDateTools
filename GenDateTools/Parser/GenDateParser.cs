﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GenDateTools.Parser
{
    public class GenDateParser : GedcomDateStringParser
    {
        private static readonly Dictionary<string, int> months = new Dictionary<string, int> { { "jan", 1 }, { "feb", 2 }, { "mar", 3 }, { "mär", 3 }, { "maa", 3 }, { "apr", 4 },
            { "mai", 5 }, { "may", 5 },  { "maj", 5 },  { "mei", 5 }, { "jun", 6 }, { "jul", 7 }, { "aug", 8 }, { "sep", 9 }, {"oct", 10 },
            {"okt", 10 }, {"nov", 11 }, {"des", 12 }, {"dec", 12 }, {"dez", 12 } };

        private static readonly string beginRx = "^";
        private static readonly string endRx = "$";
        private static readonly string sepSpaceRx = @"[\s]+";
        private static readonly string sepIsoRx = @"-";
        private static readonly string sepDotRx = @"\.";
        private static readonly string dayRx = @"(?<day>\d{1,2})";
        private static readonly string monthNumRx = @"(?<month>\d{1,2})";
        private static readonly string monthRx = @"(?<month>" + string.Join("|", months.Keys) + ")";
        private static readonly string yearRx = @"(?<year>\d{1,4})";

        private static readonly Regex regexDayMonYear = new Regex(beginRx + dayRx + sepSpaceRx + monthRx + sepSpaceRx + yearRx + endRx);
        private static readonly Regex regexMonYear = new Regex(beginRx + monthRx + sepSpaceRx + yearRx + endRx);
        private static readonly Regex regexNor = new Regex(beginRx + dayRx + sepDotRx + monthNumRx + sepDotRx + yearRx + endRx);
        private static readonly Regex regexIso = new Regex(beginRx + yearRx + sepIsoRx + monthNumRx + sepIsoRx + dayRx + endRx);
        private static readonly Regex regexIsoMonYear = new Regex(beginRx + yearRx + sepIsoRx + monthNumRx + endRx);
        private static readonly Regex regexYear = new Regex(beginRx + yearRx + endRx);

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
            var mDayMonYear = regexDayMonYear.Match(sDate);
            if (mDayMonYear.Success && months.ContainsKey(mDayMonYear.Groups["month"].Value))
            {
                return new DatePart(mDayMonYear.Groups["year"].Value, months[mDayMonYear.Groups["month"].Value].ToString(), mDayMonYear.Groups["day"].Value);
            }

            // Match on month year
            var mMonYear = regexMonYear.Match(sDate);
            if (mMonYear.Success && months.ContainsKey(mMonYear.Groups["month"].Value))
            {
                return new DatePart(mMonYear.Groups["year"].Value, months[mMonYear.Groups["month"].Value].ToString(), "0");
            }

            // Match on dd.mm.yyyy
            var mNor = regexNor.Match(sDate);
            if (mNor.Success)
            {
                return new DatePart(mNor.Groups["year"].Value, mNor.Groups["month"].Value, mNor.Groups["day"].Value);
            }

            // Match on ISO
            var mIso = regexIso.Match(sDate);
            if (mIso.Success)
            {
                return new DatePart(mIso.Groups["year"].Value, mIso.Groups["month"].Value, mIso.Groups["day"].Value);
            }

            // Match on ISO year and month
            var mIsoYM = regexIsoMonYear.Match(sDate);
            if (mIsoYM.Success)
            {
                return new DatePart(mIsoYM.Groups["year"].Value, mIsoYM.Groups["month"].Value, "0");
            }

            // Match on year
            var mYear = regexYear.Match(sDate);
            if (mYear.Success)
            {
                return new DatePart(mYear.Groups["year"].Value, "0", "0");
            }

            return new DatePart();
        }
    }
}
