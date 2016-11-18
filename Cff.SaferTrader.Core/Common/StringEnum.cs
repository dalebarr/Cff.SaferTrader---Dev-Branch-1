using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core.Common
{ //CFFWEB-7 (added this attribute for myob authentication usage)
    [Serializable]
    public class StringValueAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
        #endregion
    }

     public enum CAUTHKEYS {
        [StringValue("NONE")]
        UNSUPPORTED=-1,

        [StringValue("KUPD")]
         KEY_UPDATE=0,

        [StringValue("KSYN")]
        KEY_SYNCH = 1,

        [StringValue("FUPL")]
        FILEKEY_UPLOAD = 2,

        [StringValue("FDL")]
        FILEKEY_DOWNLOAD = 3,

        [StringValue("CONL")]
        CLIENT_IS_ONLINE = 4,

        [StringValue("COFL")]
        CLIENT_IS_OFFLINE = 5
    }


     public enum MYOB_ENCTYPES : short { 
        [StringValue("3DES")]
         TRIPLE_DES=1,
         [StringValue("RIJNDAEL")]
         RIJNDAEL=2
     }

     public enum MYOB_TRXTYPE : short { //transaction type
        [StringValue("EXPORT")]
         EXPORT=0,
        [StringValue("IMPORT")]
         IMPORT=1
     }


     public enum MYOB_KUPDATE_TYPE
     { 
        TRIGGER_KEY_SYNC=0,
        RETRIEVE_FILEKEYS=1,
        DELETE_FILEKEYS=2,
        UNSUPPORTED = -1
     }

    public static class StringEnum
     {
         public static string GetStringValue(Enum value)
         {
             // Get the type
             Type type = value.GetType();

             // Get fieldinfo for this type
             System.Reflection.FieldInfo fieldInfo = type.GetField(value.ToString());

             // Get the stringvalue attributes
             StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                 typeof(StringValueAttribute), false) as StringValueAttribute[];

             // Return the first if there was a match.
             return attribs.Length > 0 ? attribs[0].StringValue : null;
         }

         public static CAUTHKEYS ParseAuthKeys(string value)
         {
             return (
                 (value == StringEnum.GetStringValue(CAUTHKEYS.KEY_UPDATE))?CAUTHKEYS.KEY_UPDATE:
                 (value == StringEnum.GetStringValue(CAUTHKEYS.KEY_SYNCH))?CAUTHKEYS.KEY_SYNCH:
                 (value == StringEnum.GetStringValue(CAUTHKEYS.FILEKEY_UPLOAD))?CAUTHKEYS.FILEKEY_UPLOAD:
                 (value == StringEnum.GetStringValue(CAUTHKEYS.FILEKEY_DOWNLOAD))?CAUTHKEYS.FILEKEY_DOWNLOAD:
                 (value == StringEnum.GetStringValue(CAUTHKEYS.CLIENT_IS_ONLINE))?CAUTHKEYS.CLIENT_IS_ONLINE:
                 (value == StringEnum.GetStringValue(CAUTHKEYS.CLIENT_IS_OFFLINE))?CAUTHKEYS.CLIENT_IS_OFFLINE:
                 CAUTHKEYS.UNSUPPORTED
                 );
         }

         public static string GenerateUniqueKey(int keyLen)
         { 
            string a = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            char[] aChar = a.ToCharArray();
            byte[] data = new byte[keyLen-1];

            System.Security.Cryptography.RNGCryptoServiceProvider theCrypt = new System.Security.Cryptography.RNGCryptoServiceProvider();
            theCrypt.GetNonZeroBytes(data);
            theCrypt.GetNonZeroBytes(data);

            StringBuilder keyRes = new StringBuilder(keyLen-1);
            foreach (byte b in data)
            {
                keyRes.Append( aChar[b%(aChar.ToString().Length -1)] );
            }


            return keyRes.ToString();
         }

    }




    
}
