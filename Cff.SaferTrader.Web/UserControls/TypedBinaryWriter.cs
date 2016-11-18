using System;
using System.IO;
using System.Text;

namespace Cff.SaferTrader.Web.UserControls.GenericGridViewControls
{
    public class TypedBinaryWriter : BinaryWriter
    {
        public TypedBinaryWriter(Stream input);
        public TypedBinaryWriter(Stream input, Encoding encoding);

        public void WriteObject(object value);
        public void WriteType(object value);
        public void WriteType(Type type);
        public void WriteTypedObject(object value);
    }
}