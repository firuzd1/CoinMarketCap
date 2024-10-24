using CoinMarketCap.Controllers;
using CoinMarketCap.Dtos;
using CoinMarketCap.Interfaces.Services;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace CoinMarketCap.Tests.Controllers
{
    public class CoinMarketCapControllerTests
    {
        private readonly Mock<ICoinMarketCapService> _mockService;
        private readonly CoinMarketCapController _controller;

        public CoinMarketCapControllerTests()
        {
            _mockService = new Mock<ICoinMarketCapService>();
            _controller = new CoinMarketCapController(_mockService.Object);
        }


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


        [Fact]
        public async Task GetAllCryptocurrenciesAsync_ReturnsListOfCryptocurrencyData()
        {
            // Arrange
            var search = "bitcoin";
            var page = 1;
            var cancellationToken = CancellationToken.None;

            var expectedCryptocurrencies = new List<CryptocurrencyData>
            {
                new CryptocurrencyData
                {
                    Id = 1,
                    Name = "Bitcoin",
                    Symbol = "BTC",
                    Slug = "bitcoin",
                    NumMarketPairs = 100,
                    DateAdded = "2013-04-28",
                    MaxSupply = 21000000,
                    CirculatingSupply = 19000000,
                    TotalSupply = 21000000,
                    CmcRank = 1,
                    Quote = new Quote
                    {
                        USD = new QuoteUSD
                        {
                            Price = 45000,
                            Volume24h = 30000000000,
                            PercentChange1h = 0.5m,
                            MarketCap = 850000000000
                        }
                    }
                }
            };

            _mockService.Setup(s => s.GetAllCryptocurrenciesAsync(page, search, cancellationToken))
                .ReturnsAsync(expectedCryptocurrencies);

            // Act
            var result = await _controller.GetAllCryptocurrenciesAsync(search, page, cancellationToken);

            // Assert
            var okResult = Assert.IsType<List<CryptocurrencyData>>(result);
            Assert.Equal(expectedCryptocurrencies.Count, okResult.Count);
            Assert.Equal(expectedCryptocurrencies[0].Name, okResult[0].Name);
            Assert.Equal(expectedCryptocurrencies[0].Symbol, okResult[0].Symbol);
        }


        [Fact]
        public async Task GetMetadataAsync_ReturnsCryptocurrencyMetaData()
        {
            // Arrange
            var symbol = CoinSymbols.BTC; // Замените на нужный символ
            var cancellationToken = CancellationToken.None;

            var expectedMetaData = new CryptocurrencyMetaData
            {
                Symbol = "BTC",
                Name = "Bitcoin",   
            };

            _mockService.Setup(s => s.GetMetadataAsync(symbol, cancellationToken))
                        .ReturnsAsync(expectedMetaData);

            // Act
            var result = await _controller.GetMetadataAsync(symbol, cancellationToken);

            // Assert
            var okResult = Assert.IsType<CryptocurrencyMetaData>(result);
            Assert.Equal(expectedMetaData.Symbol, okResult.Symbol);
            Assert.Equal(expectedMetaData.Name, okResult.Name);
            // Проверьте другие необходимые поля...
        }
    }
}