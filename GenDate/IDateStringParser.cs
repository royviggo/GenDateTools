namespace GenDate
{
    public interface IDateStringParser
    {
        GenDate Parse(string dateString);
        DatePart GetDatePartFromStringDate(string sDate);
    }
}