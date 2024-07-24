using BnpTest.Core.Contracts.Services;
using BnpTest.Core.Contracts.ExternalServices;
using BnpTest.Service;
using Moq;
using BnpTest.Core.Contracts.Repository;
using Microsoft.Extensions.Logging;
using BnpTest.Core.Entities;

namespace BnpTest.Tests
{
    public class SecurityServiceTest
    {
        private readonly Mock<ILogger<SecurityService>> _logger;
        private readonly ISecurityService _sut;
        private readonly Mock<IExternalServices> _externalServices;
        private readonly Mock<ISecurityRepository> _securityRepository;

        public SecurityServiceTest()
        {
            _externalServices = new Mock<IExternalServices>();
            _securityRepository = new Mock<ISecurityRepository>();
            _logger = new Mock<ILogger<SecurityService>>();
            _sut = new SecurityService(_securityRepository.Object, _externalServices.Object, _logger.Object);
        }

        [Fact]
        public void ProcessIsinShouldBeProcessedWhenDataIsValid()
        {
            //Arrange
            List<string> codes = new List<string>() {
                "1d65sa4dasdd",
          };

            _externalServices.Setup(x => x.GetPrice(It.IsAny<string>())).ReturnsAsync(12.000m);

            //Act

            _sut.ProcessIsin(codes).Wait();

            //Assert
            _securityRepository.Verify(s => s.CreateAsync(It.IsAny<Security>()), Times.Once);
            _logger.Verify(x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("All 1 where processed succesfully")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public void ProcessIsinSholdFailWhenCodeIsEmptyString()
        {
            //Arrange
            List<string> codes = new List<string> { "", "  ", };

            //Act

            _sut.ProcessIsin(codes).Wait();

            _securityRepository.Verify(x => x.CreateAsync(It.IsAny<Security>()), Times.Never);
            _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Invalid format for isin code")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(2));
        }
    }
}