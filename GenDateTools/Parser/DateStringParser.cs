using System;

namespace GenDateTools.Parser
{
    public class DateStringParser : IDateStringParser
    {
        public virtual GenDate Parse(string dateString)
        {
            if (dateString.Length >= 18)
            {
                if (Enum.TryParse(dateString.Substring(9, 1), out GenDateType dateTypeOut))
                {
                    var fromDate = GetDatePartFromStringDate(dateString.Substring(1, 8));
                    var toDate = GetDatePartFromStringDate(dateString.Substring(10, 8));

                    return new GenDate(dateTypeOut, fromDate, toDate, true);
                }
            }

            return new GenDate(GenDateType.Invalid, dateString, false);
        }

        public virtual DatePart GetDatePartFromStringDate(string sDate)
        {
            return new DatePart(sDate);
        }
    }
}