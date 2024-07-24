namespace BnpTest.Core.Contracts.ExternalServices
{
    public interface IExternalServices
    {
        Task<decimal> GetPrice(string isin);
    }
}
