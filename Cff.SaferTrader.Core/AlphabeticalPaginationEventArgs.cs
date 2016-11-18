using System;

namespace Cff.SaferTrader.Core
{
    public class AlphabeticalPaginationEventArgs:EventArgs
    {
        private readonly string letter;

        public AlphabeticalPaginationEventArgs(string letter)
        {
            this.letter = letter;
        }

        public string Letter
        {
            get { return letter; }
        }
    }
}