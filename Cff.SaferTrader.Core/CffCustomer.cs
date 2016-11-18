using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class CffCustomer 
    {
        private readonly int id;
        private readonly string name;
        private readonly int number;

        public CffCustomer(string name, int id, int number)
        {
            this.name = name;
            this.number = number;
            this.id = id;
        }

        public int Id
        {
            get { return id; }
        }

        public int Number
        {
            get { return number; }
        }

        public string Name
        {
            get { return name; }
        }

    
        public string NameAndNumber
        {
          //get { return string.Format("{0}({1})", name, number); }
          get { return string.Format("{0}", name); }
        }

        public string NameAndNumberJSON()
        {
            //return string.Format("{{customerName:'{0}',customerId: '{1}'}}\n", customerNameAndNum, Id);
            //return string.Format("{{\"label\":\"{0}\", \"value\": \"{1}\"}}", customerNameAndNum, Id);
            //string customerNameAndNum = NameAndNumber.Replace("'", "\\'").Replace("\"", "\\\"").Replace("/", "\\/").Replace("&", "\\&"); //.Replace("(", "\\(").Replace(")", "\\)")
            //MSarza: added replace param
            //return "{\"label\":\"" + NameAndNumber.Replace(":","").Replace("#","").Replace("\\","") + "\",\"value\": \"" + Id + "\"}";
            return "{\"label\":\"" + NameAndNumber.Replace(":", "").Replace("#", "").Replace("\\", "").Replace("\"","'") + "\",\"value\": \"" + Id + "\"}";
        }
    }
}