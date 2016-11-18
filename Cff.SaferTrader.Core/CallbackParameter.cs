using System;

namespace Cff.SaferTrader.Core
{
    public class CallbackParameter
    {
        private readonly string fieldName;
        private readonly int rowIndex;
        private const char Separator= ';';

        public CallbackParameter(string fieldName, int rowIndex)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(fieldName, "fieldName");
            ArgumentChecker.ThrowIfLessThanZero(rowIndex, "rowIndex");

            this.fieldName = fieldName;
            this.rowIndex = rowIndex;
        }

        public override string ToString()
        {
            return fieldName + Separator+ rowIndex;
        }

        public static CallbackParameter Parse(string value)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(value, "value");
            string[] splitValue = value.Split(Separator);
            if (splitValue.Length != 2)
            {
                throw new ArgumentException("Unrecognized CallbackParameter format");
            }

            return new CallbackParameter(splitValue[0], int.Parse(splitValue[1]));
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public int RowIndex
        {
            get { return rowIndex;}
        }
    }
}