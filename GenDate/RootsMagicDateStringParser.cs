using System.Text.RegularExpressions;

namespace GenDate
{
    public class RootsMagicDateStringParser : DateStringParser
    {
        public override GenDate Parse(string dateString)
        {
            var regex = new Regex(@"^(?<stype>D)(?<dtype>.).+(?<fyear>\d{4})(?<fmonth>\d{2})(?<fday>\d{2}).(?<dstype>\S).(?<tyear>\d{4})(?<tmonth>\d{2})(?<tday>\d{2})..");
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

                var fromDate = new DatePart(m.Groups["fyear"].Value, m.Groups["fmonth"].Value, m.Groups["fday"].Value);
                var toDate = (dateType == GenDateType.Between || dateType == GenDateType.FromTo)
                             ? new DatePart(m.Groups["tyear"].Value, m.Groups["tmonth"].Value, m.Groups["tday"].Value)
                             : fromDate;

                return new GenDate(dateType, fromDate, toDate, true);
            }

            var datePhrase = dateString.Length > 1 ? dateString.Substring(1, dateString.Length - 1) : "";

            return new GenDate(GenDateType.Invalid, datePhrase, false);
        }
    }
}
