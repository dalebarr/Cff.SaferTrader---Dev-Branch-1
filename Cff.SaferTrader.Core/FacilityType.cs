using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Core
{
    public class FacilityType
    {
        private static readonly FacilityType AllFacilityType = new FacilityType(0, "All");
        private static readonly FacilityType FactoringFacilityType = new FacilityType(1, "Factoring");
        private static readonly FacilityType DiscountingFacilityType = new FacilityType(2, "Debtor Mgt");
        private static readonly FacilityType StockFundingFacilityType = new FacilityType(3, "CFSL Funding");
        private static readonly FacilityType LoanFacilityType = new FacilityType(4, "Loan");
        private static readonly FacilityType OtherFacilityType = new FacilityType(5, "Current A/c");
        private static readonly FacilityType FactoringAndDiscountingFacilityType = new FacilityType(6, "Factoring and Discounting");
        private static readonly FacilityType NotFactoringOrDiscountingFacilityType = new FacilityType(7, "Not Factoring or Discounting");
        private static readonly Dictionary<int, FacilityType> FacilityTypes = InitialiseFacilityTypes();
        private static readonly Dictionary<int, FacilityType> FactoringOrDiscountingFacilityTypes = InitialiseFactoringOrDiscountingFacilityTypes();

        private readonly int id;
        private readonly string name;

        private FacilityType(int id, string name)
        {
            ArgumentChecker.ThrowIfLessThanZero(id, "id");
            ArgumentChecker.ThrowIfNullOrEmpty(name, "name");

            this.id = id;
            this.name = name;
        }

        private static Dictionary<int, FacilityType> InitialiseFactoringOrDiscountingFacilityTypes()
        {
            var factoringOrDiscountingFacilityTypes = new Dictionary<int, FacilityType>
                                      {
                                          {AllFacilityType.Id, AllFacilityType},
                                          {FactoringFacilityType.Id, FactoringFacilityType},
                                          {DiscountingFacilityType.Id, DiscountingFacilityType},
                                          {FactoringAndDiscountingFacilityType.Id, FactoringAndDiscountingFacilityType}
                                      };
            return factoringOrDiscountingFacilityTypes;
        }

        private static Dictionary<int, FacilityType> InitialiseFacilityTypes()
        {
            var knownFacilityTypes = new Dictionary<int, FacilityType>
                                      {
                                          {AllFacilityType.Id, AllFacilityType},
                                          {FactoringFacilityType.Id, FactoringFacilityType},
                                          {DiscountingFacilityType.Id, DiscountingFacilityType},
                                          {StockFundingFacilityType.Id, StockFundingFacilityType},
                                          {LoanFacilityType.Id, LoanFacilityType},
                                          {OtherFacilityType.Id, OtherFacilityType},
                                          {FactoringAndDiscountingFacilityType.Id, FactoringAndDiscountingFacilityType},
                                          {NotFactoringOrDiscountingFacilityType.Id, NotFactoringOrDiscountingFacilityType}
                                      };
            return knownFacilityTypes;
        }

        public static FacilityType Parse(int selectedFacilityTypeId)
        {
            FacilityType facilityType;
            if (FacilityTypes.ContainsKey(selectedFacilityTypeId))
            {
                facilityType = FacilityTypes[selectedFacilityTypeId];
            }
            else
            {
                throw new InvalidOperationException("Specified Facility Type ID does not exist");
            }
            return facilityType;
        }

        /// <summary>
        /// Returns All, Factoring, Discounting, and Factoring and Discounting FacilityTypes
        /// </summary>
        public static Dictionary<int, FacilityType> FactoringOrDiscountingTypes
        {
            get { return FactoringOrDiscountingFacilityTypes; }
        }

        /// <summary>
        /// Returns All, Factoring, Discounting, and Factoring and Discounting FacilityTypes as ListItems
        /// </summary>
        public static List<ListItem> FactoringOrDiscountingTypesAsListItems
        {
            get
            {
                var facilityTypeListItems = new List<ListItem>();
                foreach (var type in FactoringOrDiscountingFacilityTypes)
                {
                    facilityTypeListItems.Add(new ListItem(type.Value.Name, type.Key.ToString()));
                }
                return facilityTypeListItems;
            }
        }

        /// <summary>
        /// Returns all known FacilityTypes
        /// </summary>
        public static Dictionary<int, FacilityType> KnownTypes
        {
            get { return FacilityTypes; }
        }

        /// <summary>
        /// Returns all known FacilityTypes as ListItems
        /// </summary>
        public static List<ListItem> KnownTypesAsListItems
        {
            get
            {
                var facilityTypeListItems = new List<ListItem>();
                foreach (var type in FacilityTypes)
                {
                    facilityTypeListItems.Add(new ListItem(type.Value.Name, type.Key.ToString()));
                }
                return facilityTypeListItems;
            }
        }

        public string Name
        {
            get { return name; }
        }

        public int Id
        {
            get { return id; }
        }

        public static FacilityType All
        {
            get { return AllFacilityType; }
        }

        public static FacilityType Factoring
        {
            get { return FactoringFacilityType; }
        }

        public static FacilityType Discounting
        {
            get { return DiscountingFacilityType; }
        }

        public static FacilityType StockFunding
        {
            get { return StockFundingFacilityType; }
        }

        public static FacilityType Loan
        {
            get { return LoanFacilityType; }
        }

        public static FacilityType Other
        {
            get { return OtherFacilityType; }
        }

        public static FacilityType FactoringAndDiscounting
        {
            get { return FactoringAndDiscountingFacilityType; }
        }

        public static FacilityType NotFactoringOrDiscounting
        {
            get { return NotFactoringOrDiscountingFacilityType; }
        }
    }
}