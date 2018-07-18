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
                GenDateType dateTypeOut;

                if (Enum.TryParse(m.Groups["dtype"].Value, out dateTypeOut))
                {
                    var fromDate = new DatePart(m.Groups["fromdate"].Value);
                    var toDate = new DatePart(m.Groups["todate"].Value);

                    return new GenDate(dateTypeOut, fromDate, toDate, true);
                }
            }
            var datePhrase = dateString.Length > 1 ? dateString.Substring(2, dateString.Length - 1) : "";

            return new GenDate(GenDateType.Invalid, datePhrase, false);
        }

        public virtual DatePart GetDatePartFromStringDate(string sDate)
        {
            var regex = new Regex(@"^(?<date>\d{8})");
            var m = regex.Match(sDate);

            return m.Success ? new DatePart(m.Groups["date"].Value)
                : new DatePart();
        }
    }
}