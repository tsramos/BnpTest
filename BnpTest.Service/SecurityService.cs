using BnpTest.Core.Contracts.ExternalServices;
using BnpTest.Core.Contracts.Repository;
using BnpTest.Core.Contracts.Services;
using BnpTest.Core.Entities;
using Microsoft.Extensions.Logging;

namespace BnpTest.Service
{
    public class SecurityService : ISecurityService
    {
        private readonly ILogger _logger;
        private readonly ISecurityRepository _securityRepository;
        private readonly IExternalServices _externalServices;

        public SecurityService(ISecurityRepository securityRepository, 
                               IExternalServices externalServices,
                               ILogger<SecurityService> logger)
        {
            _logger = logger;
            _securityRepository = securityRepository;
            _externalServices = externalServices;
        }

        public async Task ProcessIsin(List<string> isinCodes)
        {
            Security security;
            foreach (var isinCode in isinCodes)
            {
                if (string.IsNullOrWhiteSpace(isinCode) || isinCode.Length != 12)
                {
                    _logger.LogError($"Invalid format for isin code - {isinCode}");
                    continue;
                }

                decimal price = await _externalServices.GetPrice(isinCode);
                security = new Security() { ISIN = isinCode, Price = price};
                await _securityRepository.CreateAsync(security);
            }

            _logger.LogInformation($"All {isinCodes.Count} where processed succesfully");
        }
    }
}
