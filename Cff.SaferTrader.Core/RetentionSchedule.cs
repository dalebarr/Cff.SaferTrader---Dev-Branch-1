using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class RetentionSchedule
    {
        private readonly int id;
        private readonly string clientName;
        private readonly Date endOfMonth;
        private readonly string status;
        private readonly IDate releaseDate;
        private readonly int clientId;
        private readonly string RetnNotes;
        private readonly int clientFacilityType;

        public RetentionSchedule(int id, string clientName, Date endOfMonth, string status, IDate releaseDate, int clientId, string RetnNotes, int clientFacilityType)
        {
            this.id = id;
            this.clientId = clientId;
            this.clientName = clientName;
            this.endOfMonth = endOfMonth;
            this.status = status;
            this.releaseDate = releaseDate;
            this.RetnNotes = RetnNotes;
            this.clientFacilityType = clientFacilityType;
        }

        public int ClientId
        {
            get { return clientId; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        public int Id
        {
            get { return id; }
        }

        public Date EndOfMonth
        {
            get { return endOfMonth; }
        }

        public string Status
        {
            get { return status; }
        }

        public IDate ReleaseDate
        {
            get { return releaseDate; }
        }

        public string Notes
        {
            get { return RetnNotes; }
        }


        public int ClientFacilityType
        {
            get { return clientFacilityType; }
        }


    }
}