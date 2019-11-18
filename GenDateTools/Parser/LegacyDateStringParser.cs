using System.Text.RegularExpressions;

namespace GenDateTools.Parser
{
    public class LegacyDateStringParser : DateStringParser
    {
        public override GenDate Parse(string dateString)
        {
            var regex = new Regex(@"^(?<dtype>.)0(?<fromdate>\d{8})(?<todate>\d{8})(?<text>.*)");
            var m = regex.Match(dateString);

            if (m.Success)
            {
                var dateTypeStr = m.Groups["dtype"].Value;
                GenDateType dateType;

                switch (dateTypeStr)
                {
                    case "0":
                    case ":":
                        dateType = GenDateType.Exact;
                        break;
                    case "1":
                    case "2":
                        dateType = GenDateType.About;
                        break;
                    case "3":
                        dateType = GenDateType.Before;
                        break;
                    case "4":
                        dateType = GenDateType.After;
                        break;
                    case "5":
                        dateType = GenDateType.Between;
                        break;
                    case "F":
                    case "M":
                        dateType = GenDateType.Period;
                        break;
                    case "h":
                        dateType = GenDateType.Calculated;
                        break;
                    case "?":
                        dateType = GenDateType.Estimated;
                        break;
                    default:
                        dateType = GenDateType.Invalid;
                        break;
                }

                var fromDate = GetDatePartFromStringDate(m.Groups["fromdate"].Value);
                var toDate = (dateType == GenDateType.Between || dateType == GenDateType.Period) ? GetDatePartFromStringDate(m.Groups["todate"].Value) : fromDate;
                var datePhrase = (m.Groups["text"].Value.Length > 1) ? m.Groups["text"].Value.Substring(1, m.Groups["text"].Value.Length - 1) : string.Empty;

                return new GenDate(dateType, fromDate, toDate, datePhrase, isValid: fromDate.IsValidDate() && toDate.IsValidDate());
            }

            return new GenDate(GenDateType.Invalid, datePhrase: "", isValid: false);
        }

        public override DatePart GetDatePartFromStringDate(string sDate)
        {
            var beginRegex = "^";
            var endRegex = "$";
            var dayRegex = @"(?<day>\d{2})";
            var monthRegex = @"(?<month>\d{2})";
            var yearRegex = @"(?<year>\d{4})";

            var regexDayMonYear = new Regex(beginRegex + dayRegex + monthRegex + yearRegex + endRegex);

            // Match on day month year
            var mDayMonYear = regexDayMonYear.Match(sDate);
            if (mDayMonYear.Success)
                return new DatePart(mDayMonYear.Groups["year"].Value, mDayMonYear.Groups["month"].Value, mDayMonYear.Groups["day"].Value);

            return new DatePart();
        }
    }
}
