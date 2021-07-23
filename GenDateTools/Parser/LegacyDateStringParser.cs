namespace GenDateTools.Parser
{
    public class LegacyDateStringParser : DateStringParser
    {
        public override GenDate Parse(string dateString)
        {
            if (dateString.Length < 18)
                return new GenDate(GenDateType.Invalid, datePhrase: "", isValid: false);

            var dateTypeStr = dateString.Substring(0, 1);
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

            var fromDate = GetDatePartFromStringDate(dateString.Substring(2, 8));
            var toDate = dateType == GenDateType.Between || dateType == GenDateType.Period ? GetDatePartFromStringDate(dateString.Substring(10, 8)) : fromDate;
            var datePhrase = dateString.Length > 18 ? dateString.Substring(19, dateString.Length - 19) : string.Empty;

            return new GenDate(dateType, fromDate, toDate, datePhrase, isValid: fromDate.IsValidDate() && toDate.IsValidDate());
        }

        public override DatePart GetDatePartFromStringDate(string sDate)
        {
            return new DatePart(sDate.Substring(4, 4), sDate.Substring(2, 2), sDate.Substring(0, 2));
        }
    }
}
