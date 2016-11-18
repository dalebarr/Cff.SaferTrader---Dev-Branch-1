namespace Cff.SaferTrader.Core
{
    public class CffClientInformation
    {
        private readonly short noCalls;
        private readonly CffClient cffClient;

        private readonly string cffLegalEntity;      //MSarza [20150730]
        private readonly string clientSignature;      //MSarza [20150810]

        public CffClientInformation(CffClient cffClient, short noCalls)
        {
            this.noCalls = noCalls;
            this.cffClient = cffClient;
        }

        //MSarza [20150730]
        public CffClientInformation(CffClient cffClient, short noCalls, string cffLegalEntity, string clientSignature)
        {
            this.noCalls = noCalls;
            this.cffClient = cffClient;
            this.cffLegalEntity = cffLegalEntity;
            this.clientSignature = clientSignature;
        }


        public CffClient CffClient
        {
            get { return cffClient; }
        }

        public short NoCalls
        {
            get { return noCalls; }
        }

        //MSarza [20150730]
        public string CffLegalEntity
        {
            get { return cffLegalEntity;  }
        }
        //MSarza [20150810]
        public string ClientSignature
        {
            get { return clientSignature; }
        }
    }
}