using System;
using System.Collections.Generic;
using System.Web;

namespace Cff.SaferTrader.Core
{  
    
    public class CffOnlineFormApplication
    {         

        public int Id { get; set; }

        public int Type { get; set; }
         
        public int ApplicantType { get; set; }
         
        public string Name { get; set; }
         
        public string CompanyNumber { get; set; }
         
        public string NZBN { get; set; }
         
        public string Telephone { get; set; }
         
        public string Email { get; set; }
         
        public bool Profitable { get; set; }
         
        public bool Liabilities { get; set; }
         
        public string PaymentPlan { get; set; }
         
        public int? NumActiveCustomers { get; set; }
         
        public decimal? LastMonthSales { get; set; }
         
        public decimal? LastYearSales { get; set; }
         
        public decimal? TotalDebtor { get; set; }
         
        public decimal? ValueStockOnHand { get; set; }
         
        public bool PreviousConvictionsOrObligations { get; set; }
         
        public List<CffOnlineApplicationDirector> Directors { get; set; }
         
        public bool Google { get; set; }
         
        public string RadioStation { get; set; }
         
        public string PaperAds { get; set; }
         
        public string Referral { get; set; }
         
        public bool InterestCoNz { get; set; }
         
        public string Other { get; set; } 
         
        public string NameSignature { get; set; }
         
        public DateTime? DateSigned { get; set; }
         
        public string SignatureFile { get; set; }
         
        public string RandomId { get; set; }

        public HttpPostedFileBase File { get; set; }

    }

    public class CffOnlineApplicationDirector
    {
         
        public string FullName { get; set; }
         
        public DateTime? DateOfBirth { get; set; }
         
        public bool Trust { get; set; }
         
        public List<CffOnlineApplicationTrust> TrustList { get; set; }
         
        public List<CffOnlineApplicationFinancialItem> FinancialList { get; set; }
         
        public decimal? OtherAsset { get; set; }
         
        public decimal? OtherLiability { get; set; }
    }

    public class CffOnlineApplicationTrust
    {
         
        public int OwnershipType { get; set; }
         
        public string Address { get; set; }
         
        public string Value { get; set; }
         
        public string Mortgage { get; set; }
    }

    public class CffOnlineApplicationFinancialItem
    {
         
        public int FinancialType { get; set; }
         
        public string Particular { get; set; }
         
        public string Asset { get; set; }
         
        public string Liability { get; set; }
    }

    
}
