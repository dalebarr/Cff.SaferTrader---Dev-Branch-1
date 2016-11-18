using System.Collections.Generic;
namespace Cff.SaferTrader.Core.Repositories
{
    public interface IDictionaryRepository
    {
        IDictionary<int, string> LoadActivityTypes();
        IDictionary<int, string> LoadNoteTypes();
    }
}