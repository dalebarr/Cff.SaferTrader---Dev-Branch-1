using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class BankDetails
    {
        private readonly string bankName;
        private readonly string branch;
        private readonly string accountNumber;

        public BankDetails(string bankName, string branch, string accountNumber)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(bankName, "bankName");
            ArgumentChecker.ThrowIfNullOrEmpty(branch, "branch");
            ArgumentChecker.ThrowIfNullOrEmpty(accountNumber, "accountNumber");

            this.bankName = bankName;
            this.branch = branch;
            this.accountNumber = accountNumber;
        }

        public string NameAndBranch
        {
            get { return bankName + " " + branch; }
        }

        public string AccountNumber
        {
            get { return accountNumber; }
        }
    }
}