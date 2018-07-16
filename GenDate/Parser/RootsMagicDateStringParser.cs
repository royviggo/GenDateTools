using GenDateTools.Models;
using System.Text.RegularExpressions;

namespace GenDateTools.Parser
{
    public class RootsMagicDateStringParser : DateStringParser
    {
        public override GenDate Parse(string dateString)
        {
            var regex = new Regex(@"^(?<stype>D)(?<dtype>.).+(?<fromdate>\d{8}).(?<dstype>\S).(?<todate>\d{8})..");
            var m = regex.Match(dateString);

            if (m.Success)
            {
                var dateTypeStr = m.Groups["dtype"].Value;
                GenDateType dateType;

                switch (dateTypeStr)
                {
                    case "B":
                        dateType = GenDateType.Before;
                        break;
                    case "A":
                        dateType = GenDateType.After;
                        break;
                    case "R":
                        dateType = GenDateType.Between;
                        break;
                    case "S":
                        dateType = GenDateType.FromTo;
                        break;
                    case ".":
                        if (m.Groups["dstype"].Value == "A")
                            dateType = GenDateType.About;
                        else if (m.Groups["dstype"].Value == "L")
                            dateType = GenDateType.Calculated;
                        else
                            dateType = GenDateType.Exact;
                        break;
                    default:
                        dateType = GenDateType.Invalid;
                        break;
                }

                var fromDate = new DatePart(m.Groups["fromdate"].Value);
                var toDate = (dateType == GenDateType.Between || dateType == GenDateType.FromTo) ? new DatePart(m.Groups["todate"].Value) : fromDate;

                return new GenDate(dateType, fromDate, toDate, true);
            }

            var datePhrase = dateString.Length > 1 ? dateString.Substring(1, dateString.Length - 1) : "";

            return new GenDate(GenDateType.Invalid, datePhrase, false);
        }
    }
}
