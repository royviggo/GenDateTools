using System.Text.RegularExpressions;

namespace GenDateTools.Parser
{
    public class GenDateParser : GedcomDateStringParser
    {
        public override GenDate Parse(string dateString)
        {
            dateString = Regex.Replace(dateString, @"\.", "");
            dateString = Regex.Replace(dateString, @"-", "AND");

            return base.Parse(dateString);
        }
    }
}
