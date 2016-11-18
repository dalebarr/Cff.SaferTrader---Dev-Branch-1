using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class AllClients : ICffClient
    {
        private int id;
        private string name;
        private int number;
        private int clientFacilityType;

        private AllClients(int id, string name, int number, int clientFacilityType)
        {
            this.id = id;
            this.name = name;
            this.number = number;
            this.clientFacilityType = clientFacilityType;
        }

        public static AllClients Create()
        {
            return new AllClients(-1, "All Clients", -1,0);
        }

        
        public int Id
        {
            get { return id; }
        }

        public string NameAndNumber
        {
            get { return name; }
        }

        public string NameAndNumberJSON()
        {
            return "{clientName:'" + "<span class=\"AllClients\">" + Name + "</span>" +
                   "',clientId: '" +Id +"'}\n";
        }

        public int Number
        {
            get { return number; }
        }

        public string Name
        {
            get { return name; }
        }

        public int ClientFacilityType
        {
            get { return clientFacilityType; }
        }



    }
}