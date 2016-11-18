using System;
using System.Data;

namespace Cff.SaferTrader.Core.Repositories
{
    public class DataRowReader : IReader
    {
        private readonly DataRowCollection rows;
        private int index = -1;
        private DataRow row;

        public DataRowReader(DataRowCollection rows)
        {
            this.rows = rows;
        }

        public int ToInteger(string fieldName)
        {
            if (row != null)
            {
                return Convert.ToInt32(row[fieldName]);
            }
            return 0;
        }

        public short ToShort(string fieldName)
        {
            return Convert.ToInt16(row[fieldName]);
        }

        public short ToSmallInteger(string fieldName)
        {
            return Convert.ToInt16(row[fieldName]);
        }



        public string ToString(string fieldName)
        {
            if (row != null) 
            {
                return row[fieldName].ToString();
            }
            return "";
        }

        public int FromBigInteger(string fieldName)
        {
            return int.Parse(row[fieldName].ToString());
        }

        private DateTime ToDateTime(string fieldName)
        {
            try
            {
                //MS[20150416] Added validation code to prevent going into exception and
                //      performance degradation
                //return DateTime.Parse(row[fieldName].ToString());
                if (row.Table.Columns.Contains(fieldName))
                {
                return DateTime.Parse(row[fieldName].ToString());
            }
                else { return DateTime.Parse(fieldName); }

            }
            catch (Exception)
            {
                try
                {
                    return DateTime.Parse(fieldName);
                }
                catch { }
            }
            finally { }
            return DateTime.MinValue;
        }

        public IDate ToNullableDate(string fieldName)
        {
            IDate date = new NullDate();
            DateTime? dateTime = ToNullableDateTime(fieldName);
            if (dateTime.HasValue)
            {
                date = new Date(dateTime.Value);
            }
            return date;
        }

        public Date ToDate(string fieldName)
        {
            try
            {
                //MS[20150416] Added validation code to prevent going into exception and
                //      performance degradation
                //return new Date(ToDateTime(row[fieldName].ToString()));
                if (row.Table.Columns.Contains(fieldName))
                {
                return new Date(ToDateTime(row[fieldName].ToString()));
            }
                else { return (new Date(ToDateTime(fieldName))); }
            }
            catch
            {
                return (new Date(ToDateTime(fieldName)));
            }
        }

        public decimal ToDecimal(string fieldName)
        {
            try
            {
                return decimal.Parse(row[fieldName].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private DateTime? ToNullableDateTime(string fieldName)
        {
            DateTime? value = null;
            try
            {
                object field = row[fieldName];
                if (field != null && !string.IsNullOrEmpty(field.ToString()))
                {
                    value = DateTime.Parse(field.ToString());
                }
            }
            catch { }
            return value;
        }

        public decimal? ToNullableDecimal(string fieldName)
        {
            decimal? value = null;
            try
            {
                object field = row[fieldName];
                if (field != null && !string.IsNullOrEmpty(field.ToString()))
                {
                    value = Convert.ToDecimal(field.ToString());
                }
            }
            catch { }
            return value;
        }

        public bool Read()
        {
            bool hasNext = HasNext;
            if (hasNext)
            {
                index++;
                row = rows[index];
            }
            return hasNext;
        }

        public void PreviousRow()
        {
            if (index > 0)
            {
                index--;
                row = rows[index];
            }
        }

        public bool HasNext
        {
            get { return index + 1 < rows.Count; }
        }

 
    }
}