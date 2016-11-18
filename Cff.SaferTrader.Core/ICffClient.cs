namespace Cff.SaferTrader.Core
{
    public interface ICffClient
    {
        int Id { get; }
        string NameAndNumber { get; }
        string NameAndNumberJSON();
        int Number { get; }
        string Name { get; }
        int ClientFacilityType { get; }
    }

}