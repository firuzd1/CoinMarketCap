using CoinMarketCap.Dtos;
using CoinMarketCap.Helpers;
using CoinMarketCap.Models.Enums;
using CoinMarketCap.Models.SettingsModels;
using CoinMarketCap.Models;
using CoinMarketCap.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoinMarketCap.Interfaces.Services
{
    public interface IIdentityService
    {
        public Task<ApiResponse> GenerationToken(UserLoginDto loginDto, CancellationToken cancellationToken = default);
    }
}
