using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Cff.SaferTrader.Web
{
    public class CFFApplicationForm
    {

        public class _GSOnlineApplicationViewModel
        {
            [Required]
            public int Type { get; set; }
            [Required]
            public int ApplicantType { get; set; }
            [Display(Name = "Applicant / Business Name")]
            [Required]
            public string Name { get; set; }
            public string CompanyNumber { get; set; }
            public string NZBN { get; set; }
            [Display(Name = "Telephone Number")]
            [Required]
            public string Telephone { get; set; }
            [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?", ErrorMessage = "Invalid Email Address format")]
            public string Email { get; set; }
            public bool Profitable { get; set; }
            public bool Liabilities { get; set; }
            public string PaymentPlan { get; set; }
            [Display(Name = "Active Customers")]
            [Required]
            public int? NumActiveCustomers { get; set; }
            [Display(Name = "Last Month Sales")]
            [Required]
            public decimal? LastMonthSales { get; set; }
            [Display(Name = "Last Year Sales")]
            [Required]
            public decimal? LastYearSales { get; set; }
            public decimal? TotalDebtor { get; set; }
            public decimal? ValueStockOnHand { get; set; }
            public bool PreviousConvictionsOrObligations { get; set; }
            public List<_GSOnlineApplicationDirector> Directors { get; set; }
            public bool Google { get; set; }
            public string RadioStation { get; set; }
            public string PaperAds { get; set; }
            public string Referral { get; set; }
            public bool InterestCoNz { get; set; }
            public string Other { get; set; }
            [Display(Name = "Name")]
            [Required]
            public string NameSignature { get; set; }
            public DateTime? DateSigned { get; set; }
            [Display(Name = "Signature")]
            [Required(ErrorMessage = "Signature is required.")]
            public string SignatureFile { get; set; }
        }

        public class _GSOnlineApplicationTrust
        {
            public int OwnershipType { get; set; }
            public string Address { get; set; }
            public string Value { get; set; }
            public string Mortgage { get; set; }
        }
        public class _GSOnlineApplicationFinancialItem
        {
            public int FinancialType { get; set; }
            public string Particular { get; set; }
            public string Asset { get; set; }
            public string Liability { get; set; }
        }
        public class _GSOnlineApplicationDirector
        {
            public string FullName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public bool Trust { get; set; }
            public List<_GSOnlineApplicationTrust> TrustList { get; set; }
            public List<_GSOnlineApplicationFinancialItem> FinancialList { get; set; }
            public decimal? OtherAsset { get; set; }
            public decimal? OtherLiability { get; set; }
        }




    }
}