using System;
using System.ComponentModel;
using System.Reflection;

namespace Cff.SaferTrader.Core
{
    //MSarza [20150901]
    public enum CffDebtorAdmin
    {
        [Description("Not set")]
        NotSet = 0,                                                     // As per original definition: ClientFinancials.CffDebtorAdmin [data type: bit] = NO EQUIVALENT
        [Description("Administered by CFF as CFF")]
        AdminByCffAsCff = 1,                                            // As per original definition: ClientFinancials.CffDebtorAdmin [data type: bit] = true
        [Description("Administered by CFF as Client")]
        AdminByCffAsClient = 2,                                         // As per original definition: ClientFinancials.CffDebtorAdmin [data type: bit] = false
        [Description("Administered by Client and by CFF")]
        BothByCffAsCffAndClientAsClient = 3,                            // As per original definition: ClientFinancials.CffDebtorAdmin [data type: bit] = false
        [Description("Administered by Client and by CFF (on behalf)")]
        BothByCffAsClientAndClientAsClient = 4,                         // As per original definition: ClientFinancials.CffDebtorAdmin [data type: bit] = false 
        [Description("Administered by Client only")]
        AdminByClientOnly = 10                                          // As per original definition: ClientFinancials.CffDebtorAdmin [data type: bit] = NO EQUIVALENT

    }

    //public enum ClientDebtorAdmin
    //{
    //    //0 = false,
    //    //1 = false,
    //    //2 = false
    //    False = CffDebtorAdmin.NotSet | CffDebtorAdmin.AdminByCffAsCff | CffDebtorAdmin.AdminByCffAsClient,
    //    True = CffDebtorAdmin.BothByCffasClientAndClientAsClient | CffDebtorAdmin.BothByCffasCffAndClientAsClient
    //}

    //public enum DebtorAdminByCff
    //{
    //    False = 0,
    //    True = 4 << 1
    //}

    public static class CffDebtorAdminHelper
    {
        public static bool ClientIsDebtorAdmin(short id)                // 3 | 4 - This is the inverse equivalent of IsClientAdminByCFF or isAdmin byCff bool variables in the old code
        {
            return (id == (short)CffDebtorAdmin.BothByCffAsCffAndClientAsClient || id == (short)CffDebtorAdmin.BothByCffAsClientAndClientAsClient) ? true : false;
        }

        public static bool CffIsDebtorAdminForClient(short id)          // 1 | 2 | 3 | 4
        {
            return (id == (short)CffDebtorAdmin.AdminByCffAsCff || id == (short)CffDebtorAdmin.AdminByCffAsClient || id == (short)CffDebtorAdmin.BothByCffAsCffAndClientAsClient || id == (short)CffDebtorAdmin.BothByCffAsClientAndClientAsClient) ? true : false;
        }

        public static bool CffIsDebtorAdminForClientAsCff(short id)     // 1 | 3
        {
            return (id == (short)CffDebtorAdmin.AdminByCffAsCff || id == (short)CffDebtorAdmin.BothByCffAsCffAndClientAsClient) ? true : false;
        }

        public static bool CffIsDebtorAdminForClientAsClient(short id)  // 2 | 4
        {
            return (id == (short)CffDebtorAdmin.AdminByCffAsClient || id == (short)CffDebtorAdmin.BothByCffAsClientAndClientAsClient) ? true : false;
        }

        public static bool CffIsSoleDebtorAdmin(short id)               
        {
            return (id == (short)CffDebtorAdmin.AdminByCffAsCff) ? true : false;
        }

        public static string GetEnumDescription1(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static string GetEnumDescription2(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetShortDesc1(short id)
        {
            switch (id)
            {
                case 0:
                    return "None; Not set.";
                case 1:
                    return "Cff as Cff";
                case 2:
                    return "CFF as Client";
                case 3:
                    return "CFF|Client as selves";
                case 4:     
                    return "CFF|Client as client";
                default:
                    return "Not defined.";
            }
        }
    }
}
