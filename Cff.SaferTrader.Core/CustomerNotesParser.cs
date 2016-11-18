namespace Cff.SaferTrader.Core
{
    public class CustomerNotesParser
    {
        public static string Parse(string notes)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(notes, "notes");

            // DM-94 In IE7, new line is \r\n. So when the string is parsed, \r\n should be treated as a single <br /> not two
            string removedNewLineAndCarrageReturn = notes.Replace("\r\n", "<br />");


            string removedNewLine = removedNewLineAndCarrageReturn.Replace("\n", "<br />");
            string removedCarrageReturn = removedNewLine.Replace("\r", "<br />");

            return removedCarrageReturn;
        }

        public static string RemoveBr(string notes)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(notes, "notes");
            return notes.Replace("<br />", "\n");
        }

        public static string RemoveHtmlBrackets(string comment)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(comment, "comment");
            string removedOpenBraket = comment.Replace("<", " ");
            return removedOpenBraket.Replace(">", " ");
        }
    }
}
