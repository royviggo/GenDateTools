namespace GenDateTools.Parser
{
    public class RootsMagicDateStringParser : DateStringParser
    {
        public override GenDate Parse(string dateString)
        {
            if (dateString.Substring(0, 1) == "T")
            {
                string datePhrase = dateString.Length > 1 ? dateString.Substring(1, dateString.Length - 1) : string.Empty;
                return new GenDate(GenDateType.Invalid, datePhrase, false);
            }

            if (dateString.Length < 23)
            {
                return new GenDate(GenDateType.Invalid, datePhrase: "", isValid: false);
            }

            string dType = dateString.Substring(1, 1);
            string dsType = dateString.Substring(12, 1);
            GenDateType dateType;

            switch (dType)
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
                    dateType = GenDateType.Period;
                    break;
                case ".":
                    if (dsType == "A")
                    {
                        dateType = GenDateType.About;
                    }
                    else if (dsType == "L")
                    {
                        dateType = GenDateType.Calculated;
                    }
                    else
                    {
                        dateType = GenDateType.Exact;
                    }

                    break;
                default:
                    dateType = GenDateType.Invalid;
                    break;
            }

            DatePart fromDate = new DatePart(dateString.Substring(3, 8));
            DatePart toDate = (dateType == GenDateType.Between || dateType == GenDateType.Period) ? new DatePart(dateString.Substring(14, 8)) : fromDate;

            return new GenDate(dateType, fromDate, toDate, true);
        }
    }
}
