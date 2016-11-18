using System;
using System.Web;

namespace Cff.SaferTrader.Core
{
    public class CffUserActivation
    {
        String _Name;
        String _MngtEmail;
        String _MngtUKey;
        String _UserEmail;
        int _Status;

        public CffUserActivation(String name, String mngtEmail, String mngtUKey, String userEmail, int status)
        {
            _Name = name;
            _MngtEmail = mngtEmail;
            _UserEmail = userEmail;
            _Status = status;
            _MngtUKey = mngtUKey;
        }

        public String Name
        {
            get { return _Name; }
        }

        public String MngtEmail
        {
            get { return _MngtEmail; }
        }

        public String MngtUKey
        {
            get { return _MngtUKey; }
        }

        public String UserEmail
        {
            get { return _UserEmail; }
        }

        public int Status
        {
            get { return _Status; }
        }
    }
}
