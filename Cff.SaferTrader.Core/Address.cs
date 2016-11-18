using System;
using System.Text;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class Address
    {
        private readonly string addressOne;
        private readonly string addressTwo;
        private readonly string addressThree;
        private readonly string addressFour;

        public Address(string addressOne, string addressTwo, string addressThree, string addressFour)
        {
            this.addressOne = addressOne;
            this.addressTwo = addressTwo;
            this.addressThree = addressThree;
            this.addressFour = addressFour;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(addressOne);

            if (!string.IsNullOrEmpty(addressTwo))
            {
                builder.Append(", " + addressTwo);
            }

            if (!string.IsNullOrEmpty(addressThree))
            {
                builder.Append(", " + addressThree);
            }

            if (!string.IsNullOrEmpty(addressFour))
            {
                builder.Append(", " + addressFour);
            }

            return builder.ToString();
        }

        public string AddressOne
        {
            get { return addressOne; }
        }

        public string AddressTwo
        {
            get { return addressTwo; }
        }

        public string AddressThree
        {
            get { return addressThree; }
        }

        public string AddressFour
        {
            get { return addressFour; }
        }
    }
}