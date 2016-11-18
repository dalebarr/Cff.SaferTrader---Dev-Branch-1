using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class TransactionStatus
    {
        private readonly int id;
        private readonly string status;
        private static readonly TransactionStatus fundedStatus = new TransactionStatus(0, "Funded & Unallocated");
        private static readonly TransactionStatus nonFundedStatus = new TransactionStatus(1, "Non Funded");
        private static readonly TransactionStatus claimedStatus = new TransactionStatus(2, "Claimed");
        private static readonly TransactionStatus unclaimedStatus = new TransactionStatus(3, "Unclaimed");
        private static readonly TransactionStatus markedStatus = new TransactionStatus(4, "Marked 4");
        private static readonly TransactionStatus allStatus = new TransactionStatus(-9, "All");
        private static readonly TransactionStatus otherStatus = new TransactionStatus(-1, "Other");

        private static readonly Dictionary<int, TransactionStatus> knownStatus = InitializeKnownStatus();

        public static TransactionStatus Parse(int id)
        {
            TransactionStatus transactionStatus = otherStatus;
            if (knownStatus.ContainsKey(id))
            {
                transactionStatus = knownStatus[id];
            }
            return transactionStatus;
        }

        private static Dictionary<int, TransactionStatus> InitializeKnownStatus()
        {
            Dictionary<int, TransactionStatus> status = new Dictionary<int, TransactionStatus>();
            status.Add(fundedStatus.id, fundedStatus);
            status.Add(markedStatus.id, markedStatus);
            status.Add(unclaimedStatus.id, unclaimedStatus);
            status.Add(nonFundedStatus.id, nonFundedStatus);
            status.Add(claimedStatus.id, claimedStatus);
            status.Add(allStatus.id, allStatus);
            return status;
        }

        private TransactionStatus(int id, string status)
        {
            this.id = id;
            this.status = status;
        }

        public int Id
        {
            get { return id; }
        }

        public string Status
        {
            get { return status;}
        }

        public static TransactionStatus Funded
        {
            get { return fundedStatus; }
        }

        public static TransactionStatus Marked
        {
            get { return markedStatus; }
        }

        public static TransactionStatus NonFunded
        {
            get { return nonFundedStatus; }
        }

        public static TransactionStatus Claimed
        {
            get { return claimedStatus; }
        }

        public static TransactionStatus All
        {
            get { return allStatus; }
        }

        /// <summary>
        /// Represents all transaction status that are not defined yet
        /// </summary>
        public static TransactionStatus Other
        {
            get { return otherStatus; }
        }

        public static TransactionStatus Unclaimed
        {
            get { return unclaimedStatus; }
        }


        /// <summary>
        /// Returns all known TransactionStatus as ListItems
        /// </summary>
        public static IList<TransactionStatus> TransactionStatusAsListItem
        {
            get
            {
                IList<TransactionStatus> tListItem = new List<TransactionStatus>();
                foreach (var type in knownStatus)
                {
                    tListItem.Add(new TransactionStatus(type.Key, type.Value.Status));
                }
                return tListItem;
            }
        }

        /// <summary>
        /// Returns All, Funded, Non Funded TransactionStatus as ListItems
        /// </summary>
        public static IList<TransactionStatus> TransactionFilterStatusAsListItem
        {
            get
            {
                IList<TransactionStatus> tListItem = new List<TransactionStatus>();
                foreach (var type in knownStatus)
                {
                    switch (type.Value.Status.Trim())
                    { 
                        case "All":
                        case "Funded":
                        case "Non Funded":
                            tListItem.Add(new TransactionStatus(type.Key, type.Value.Status));
                            break;
                        default:
                            break;
                    }
                }
                return tListItem;
            }
        }
    }
}
