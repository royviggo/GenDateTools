using System;
using System.Text.RegularExpressions;

namespace GenDateTools.Parser
{
    public class DateStringParser : IDateStringParser
    {
        public virtual GenDate Parse(string dateString)
        {
            var regex = new Regex(@"^(?<stype>\d)(?<fromdate>\d{8})(?<dtype>\d)(?<todate>\d{8})");
            var m = regex.Match(dateString);

            if (m.Success)
            {
                if (Enum.TryParse(m.Groups["dtype"].Value, out GenDateType dateTypeOut))
                {
                    var fromDate = GetDatePartFromStringDate(m.Groups["fromdate"].Value);
                    var toDate = GetDatePartFromStringDate(m.Groups["todate"].Value);

                    return new GenDate(dateTypeOut, fromDate, toDate, true);
                }
            }
            var datePhrase = dateString.Length > 1 ? dateString.Substring(1, dateString.Length - 1) : string.Empty;

            return new GenDate(GenDateType.Invalid, datePhrase, false);
        }

        public virtual DatePart GetDatePartFromStringDate(string sDate)
        {
            return new DatePart(sDate);
        }
    }
}