namespace BnpTest.Core.Contracts.Services
{
    public interface ISecurityService
    {
        Task ProcessIsin(List<string> isinCodes);
    }
}
