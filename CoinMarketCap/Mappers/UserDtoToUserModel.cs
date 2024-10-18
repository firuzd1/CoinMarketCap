using CoinMarketCap.Dtos;
using CoinMarketCap.Models;
using CoinMarketCap.Models.Enums;

namespace CoinMarketCap.Mappers
{
    public static class UserMapper
    {
        public static UserModel UserDtoToUserModel(this UserDto userDto, Lang lang)
        {
            return new()
            {
               Login = userDto.Login,
               Password = userDto.Password,
               Lang = lang,
            };
        }


        public static UserModel UpdateUserDtoToUserModel(this UserDto userDto, int userId, Lang lang)
        {
            return new()
            {
                Id = userId,
                Login = userDto.Login,
                Password = userDto.Password,
                Lang = lang,
            };
        }
    }
}
