using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoinMarketCap.Controllers;
using CoinMarketCap.Models;
using CoinMarketCap.Services;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CoinMarketCap.Dtos;
using CoinMarketCap.Interfaces.Services;

public class TransactionControllerTests
{
    private readonly Mock<ITransactionService> _mockTransactionService;
    private readonly TransactionController _controller;

    public TransactionControllerTests()
    {
        // Создание мока сервиса
        _mockTransactionService = new Mock<ITransactionService>();

        // Инициализация контроллера с мок-сервисом
        _controller = new TransactionController(_mockTransactionService.Object);

        // Настройка контекста пользователя
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("UserId", "1")
        }));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task CheckBalanceAsync_ReturnsBalanceResponse()
    {
        // Настройка мока для возврата ожидаемых данных
        var expectedResponse = new BalanceResponse
        {
            Balance = "$1000.50",
            Pagination = new PaginationViewResponse<TransactionModel>
            {
                CurrentPage = 1,
                CountPage = 1,
                Enttities = new List<TransactionModel> { new TransactionModel { Amount = 1000.50m } }
            }
        };

        _mockTransactionService
            .Setup(service => service.CheckBalanceAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Вызов метода контроллера
        var result = await _controller.CheckBalanceAsync(null, 1);

        // Проверка результата
        Assert.NotNull(result);  // Результат не должен быть null
        Assert.Equal("$1000.50", result.Balance);  // Проверяем баланс
        Assert.IsType<BalanceResponse>(result);  // Проверяем тип возвращаемого значения
    }
}
