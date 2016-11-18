using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class CffClient : ICffClient
    {
        private readonly int id;
        private readonly string name;
        private readonly int number;
        private readonly int clientFacilityType;
        private readonly string collectionsBankAccount;
        private readonly short cffDebtorAdmin;          //MSazra [20151006]
        private readonly bool hasOwnLetterTemplates;    //MSazra [20151006]

        public CffClient(string name, int id, int number
                            , int clientFacilityType
                            , string collectionsBankAccount
                            )
        {
            this.name = name;
            this.id = id;
            this.number = number;
            this.clientFacilityType = clientFacilityType;
            this.collectionsBankAccount = collectionsBankAccount;

        }

        public CffClient(string name, int id, int number
                            , int clientFacilityType
                            , string collectionsBankAccount
                            , short cffDebtorAdmin          //MSazra [20151006]
                            , bool hasOwnLetterTemplates    //MSazra [20151006]
                            )
        {
            this.name = name;
            this.id = id;
            this.number = number;
            this.clientFacilityType = clientFacilityType;
            this.collectionsBankAccount = collectionsBankAccount;
            this.cffDebtorAdmin = cffDebtorAdmin;
            this.hasOwnLetterTemplates = hasOwnLetterTemplates;
        }
       
        public int Id
        {
            get { return id; }
        }

        public string NameAndNumber
        {
            get
            {
                return string.Format("{0}({1})", name, number);
            }
        }
        public string NameAndNumberJSON()
        {
            return "{\"label\":\"" + NameAndNumber.Replace("'", "").Replace("\"", "").Replace("\\","") + "\",\"value\": \"" + Id + "\"}"; //just clear these
            /*"{\"clientName\":\"" + NameAndNumber.Replace("'", "\\'").Replace("\"", "\\\"").Replace("(", "\\(").Replace(")", "\\)")
                      + "\",\"clientId\": \"" + Id + "\"}";*/
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

        public string CollectionsBankAccount
        {
            get { return collectionsBankAccount; }
        }

        public short DebtorAdministrationType       //MSazra [20151006]
        {
            get { return this.cffDebtorAdmin; }
        }

        public bool HasOwnLetterTemplates           //MSazra [20151006]
        {
            get { return this.hasOwnLetterTemplates; }
        }
    }

    [Serializable]
    public class CffLoginAccount : ICffLoginAccount
    {
        private readonly string username;
        private readonly string password;
        public CffLoginAccount(string uname, string pwd)
        {
            this.username = uname;
            this.password = pwd;
        }

        public string Username
        {
            get { return username; }
        }

        public string Password
        {
            get { return password; }
        }
    }

    [Serializable]
    public class UserSpecialAccounts : IUserSpecialAccounts
    {
        private readonly Guid _uid;
        private readonly string _name;
        private readonly bool _isClient;
        private readonly Int64 _id;
        private readonly bool _isLockedOut;

        public UserSpecialAccounts(Guid uid, string name, Boolean isClient, Int64 Id, bool isLockedOut)
        {
            this._uid = uid;
            this._name = name;
            this._isClient = isClient;
            this._id = Id;
            this._isLockedOut = isLockedOut;
        }
        public Guid ID { get { return _uid; } }
        public string Name { get { return _name; } }
        public Boolean IsClient { get { return _isClient; } }
        public Int64 cId { get { return _id; } }
        public bool IsLockedOut { get { return _isLockedOut; }  } 
    }
}