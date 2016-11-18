using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core
{
    public interface ICffCustomer
    {
        int Id { get; }
        string Name { get; }
        int Number { get; }
        string NameAndNumber { get; }
        string NameAndNumberJSON();
    }
}