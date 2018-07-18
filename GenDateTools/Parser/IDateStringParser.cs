namespace GenDateTools.Parser
{
    public interface IDateStringParser
    {
        GenDate Parse(string dateString);
        DatePart GetDatePartFromStringDate(string sDate);
    }
}