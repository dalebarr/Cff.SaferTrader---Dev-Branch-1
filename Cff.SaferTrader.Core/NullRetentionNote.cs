namespace Cff.SaferTrader.Core
{
    public class NullRetentionNote : IRetentionNote
    {
        private const string NullNote = "No data to display";
        public string GetNote()
        {
            return NullNote;
        }
    }
}