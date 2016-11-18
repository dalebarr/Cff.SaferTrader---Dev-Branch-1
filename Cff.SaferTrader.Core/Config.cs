using Cff.SaferTrader.Core.Properties;

namespace Cff.SaferTrader.Core
{
    public static class Config
    {
        public static string BatchDetailsPage
        {
            get { return Settings.Default.BatchDetailsPage; }
        }

        public static string DashboardPage
        {
            get { return Settings.Default.DashboardPage; }
        }

        public static int NumberOfClientsToReturn
        {
            get { return Settings.Default.NumberOfClientsToReturn; }
        }

        public static int NumberOfCustomersToReturn
        {
            get { return Settings.Default.NumberOfCustomersToReturn; }
        }

        public static string TransactionSearchPage
        {
            get { return Settings.Default.TransactionSearchPage; }
        }
    }
}