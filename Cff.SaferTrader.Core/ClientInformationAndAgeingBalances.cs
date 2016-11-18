namespace Cff.SaferTrader.Core
{
    public class ClientInformationAndAgeingBalances
    {
        private readonly AgeingBalances ageingBalances;
        private readonly ClientInformation clientInformation;
        private readonly TransactionSummary transacitonSummary;

        public ClientInformationAndAgeingBalances(AgeingBalances ageingBalances, ClientInformation clientInformation, TransactionSummary transacitonSummary)
        {
            ArgumentChecker.ThrowIfNull(ageingBalances, "ageingBalances");
            ArgumentChecker.ThrowIfNull(clientInformation, "clientInformation");
            ArgumentChecker.ThrowIfNull(transacitonSummary, "transacitonSummary");

            this.ageingBalances = ageingBalances;
            this.clientInformation = clientInformation;
            this.transacitonSummary = transacitonSummary;
        }

        public TransactionSummary TransacitonSummary
        {
            get { return transacitonSummary; }
        }

        public ClientInformation ClientInformation
        {
            get { return clientInformation; }
        }

        public AgeingBalances AgeingBalances
        {
            get { return ageingBalances; }
        }
    }
}