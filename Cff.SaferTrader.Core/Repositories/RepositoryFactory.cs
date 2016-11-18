using System.Configuration;

namespace Cff.SaferTrader.Core.Repositories
{
    public sealed class RepositoryFactory
    {
        private static readonly ICalendar Calendar = new Calendar();
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        public static IRetentionNotesTabRepository CreateRetentionNotesTabRepository()
        {
            return new RetentionNotesTabRepository(ConnectionString);
        }

        public static ICffUserRepository CreateCffUserRepository()
        {
            return new CffUserRepository(ConnectionString);
        }

        public static ITransactionRepository CreateTransactionRepository()
        {
            return new TransactionRepository(Calendar);
        }

        public static ICustomerRepository CreateCustomerRepository()
        {
            return new CustomerRepository(ConnectionString);
        }

        public static IClientRepository CreateClientRepository()
        {
            return new ClientRepository(ConnectionString);
        }

        public static IUserClientsRepository CreateUserClientsRepository()
        {
            return new UserClientsRepository(ConnectionString);
        }

        public static IReportRepository CreateReportRepository()
        {
            return new ReportRepository(Calendar, ConnectionString);
        }

        public static IDictionaryRepository CreateDictionaryRepository()
        {
            return new DictionaryRepository(ConnectionString);
        }

        public static INotesRepository CreateCustomerNotesRepository()
        {
            return new NotesRepository(ConnectionString);
        }

        public static ITransactionSearchRepository CreateTransactionSearchRepository()
        {
            return new TransactionSearchRepository(ConnectionString);
        }

        public static IBatchRepository CreateBatchRepository()
        {
            return new BatchRepository(ConnectionString);
        }

        public static IRetentionRepository CreateRetentionRepository()
        {
            return new RetentionRepository(ConnectionString);
        }

        public static IContactsRepository CreateContactsRepository()
        {
            return new ContactsRepository(ConnectionString);
        }

        public static IManagementRepository CreateManagementRepository()
        {
            return new ManagementRepository(ConnectionString);
        }

        //MSarzea [20150731] 
        public static ICffMgtDetails CreateCffMgtDetailsRepository()
        {
            return new CffMgtDetailsRepository(ConnectionString);
        }
    }
}