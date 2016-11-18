using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cff.SaferTrader.Core
{
    public interface ISessionCache
    {
        object CacheObject { get; set; }
    }

    public class CffSessionCache : ISessionCache
    {
        public object CacheObject { get; set; }
        public CffSessionCache() { 
        }
    }
}
