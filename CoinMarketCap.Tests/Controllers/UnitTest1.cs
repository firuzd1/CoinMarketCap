using CoinMarketCap.Controllers;
using CoinMarketCap.Interfaces.Services;
using CoinMarketCap.Models;
using CoinMarketCap.Services;
using Moq;

namespace CoinMarketCap.Tests.Controllers
{
    public class CoinMarketCapControllerTests
    {
        [Fact]
        public async Task GetAllCryptocurrenciesAsync_ReturnsListOfCryptocurrencies()
        {
            // Arrange
            var mockService = new Mock<ICoinMarketCapService>();
            mockService.Setup(service => service.GetAllCryptocurrenciesAsync(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new List<CryptocurrencyData>());

            var controller = new CoinMarketCapController(mockService.Object);

            // Act
            var result = await controller.GetAllCryptocurrenciesAsync(null, 1);

            // Assert
            Assert.IsType<List<CryptocurrencyData>>(result);
        }
    }
}