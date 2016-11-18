namespace Cff.SaferTrader.Core
{
    public class RetentionNote : IRetentionNote
    {
        private readonly string _notes;
        public RetentionNote(string notes)
        {
            _notes = notes;
        }
        public string GetNote()
        {
            return _notes;
        }
    }
}