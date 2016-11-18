using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class RetentionAdjustments
    {
        private readonly int id;
        private readonly string clientName;
        private readonly Date endOfMonth;
        private readonly string status;
        private readonly IDate releaseDate;
        private readonly int clientId;

        public RetentionAdjustments(int id, string clientName, Date endOfMonth, string status, IDate releaseDate, int clientId)
        {
            this.id = id;
            this.clientId = clientId;
            this.clientName = clientName;
            this.endOfMonth = endOfMonth;
            this.status = status;
            this.releaseDate = releaseDate;
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

    }
}
