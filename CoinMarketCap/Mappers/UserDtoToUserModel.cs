using CoinMarketCap.Dtos;
using CoinMarketCap.Models;

namespace CoinMarketCap.Mappers
{
    public static class UserMapper
    {
        public static UserModel UserDtoToUserModel(this UserDto userDto)
        {
            return new()
            {
               Login = userDto.Login,
               Password = userDto.Password,
            };
        }
    }
}
