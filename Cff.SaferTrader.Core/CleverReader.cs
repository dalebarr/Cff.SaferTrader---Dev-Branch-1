using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
namespace Cff.SaferTrader.Core
{
    public class CleverReader : IReader
    {
        private readonly SqlDataReader _reader;
        //private readonly DataTable _schema;
        //private readonly SqlDataReader _reader2;
        public CleverReader(SqlDataReader reader)
        {
            _reader = reader;
            //_reader2 = reader;
            //_schema = reader.GetSchemaTable();
        }

        public string ToString(string fieldName)
        {
            string s = null;
            try
            {
                int ordinal = _reader.GetOrdinal(fieldName);
                if (!_reader.IsDBNull(ordinal))
                {
                    s = _reader.GetString(ordinal);
                }
            }
            catch (Exception)
            {
                s = "";
            }
         
            return IsDbNull(s);
        }

        public bool ToBoolean(string fieldName)
        {
            try
            {
                return _reader.GetSqlBoolean(_reader.GetOrdinal(fieldName)).Value;
            }
            catch {
                return false;
            }
        }

        private static string IsDbNull(SqlString theString)
        {
            if (theString.IsNull)
            {
                return string.Empty;
            }
            return theString.ToString();
        }

        public short ToSmallInteger(string fieldName)
        
        {
            try
            {
                return _reader.GetSqlInt16(_reader.GetOrdinal(fieldName)).Value;
            }
            catch (Exception) 
            {
                return 0;
            }
        }

        public int ToInteger(string fieldName)
        {
            try
            {
                return _reader.GetSqlInt32(_reader.GetOrdinal(fieldName)).Value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int FromBigInteger(string fieldName)
        {      
            try
            {
                //System.Diagnostics.Debug.WriteLine("CleverReader.FromBigInteger: " + fieldName.ToString());
                return (int)_reader.GetSqlInt64(_reader.GetOrdinal(fieldName)).Value;
            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.WriteLine("EXCEPTION: CleverReader.FromBigInteger: " + fieldName.ToString());
                return 0;            
            }
        }

        public double ToDouble(string fieldName)
        {
            try
            {
                return _reader.GetSqlDouble(_reader.GetOrdinal(fieldName)).Value;
            }
            catch (Exception) {
                return 0;
            }
        }

        public Date ToDate(string fieldName)
        {
            try
            {
                DateTime datetime = (DateTime)(ToDateTime(fieldName) ?? null);
                return new Date(datetime);
            }
            catch {
                return Date.MinimumDate;
            }
        }

        public IDate ToNullableDate(string fieldName)
        {
            IDate date = new NullDate();
            int ordinal = _reader.GetOrdinal(fieldName);
            if (!_reader.IsDBNull(ordinal) && !string.IsNullOrEmpty(_reader.GetValue(ordinal).ToString()))
            {
                date = ToDate(fieldName);
            }
            return date;
        }

        private DateTime? ToDateTime(string fieldName)
        {
            try
            {
                return _reader.GetSqlDateTime(_reader.GetOrdinal(fieldName)).Value;
            }
            catch {
                return null;
            }
        }

        public DateTime? ToDateTimeWithMinimum(string fieldName)
        {
            try
            {
                if (_reader.GetSqlDateTime(_reader.GetOrdinal(fieldName)).IsNull)
                {
                    return DateTime.MinValue;
                }
                return _reader.GetSqlDateTime(_reader.GetOrdinal(fieldName)).Value;
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Null is represented as 0
        /// </summary>
        public int NullableIntegerToInteger(string fieldName)
        {
            if (_reader.IsDBNull(_reader.GetOrdinal(fieldName)))
            {
                return 0;
            }
            return _reader.GetSqlInt32(_reader.GetOrdinal(fieldName)).Value;
        }

        public bool Read()
        {
            return _reader.Read();
        }

        public bool NextResult()
        {
            return _reader.NextResult();
        }

        public bool HasRows
        {
            get { return _reader.HasRows; }
        }
        public bool IsNull
        {
            get { return _reader == null; }
        }

        public decimal ToDecimal(string fieldName)
        {
            try
            {
                return _reader.GetSqlDecimal(_reader.GetOrdinal(fieldName)).Value;
            }
            catch {
                return 0;
            }
        }

        public decimal FromMoney(string fieldName)
        {
            try
            {
                return _reader.GetSqlMoney(_reader.GetOrdinal(fieldName)).Value;
            }
            catch {
                return 0;
            }
        }

        public Guid ToGuid(string fieldName)
        {
            try
            {
                return _reader.GetSqlGuid(_reader.GetOrdinal(fieldName)).Value;
            }
            catch {
                return Guid.Empty;
            }
        }
    }
}