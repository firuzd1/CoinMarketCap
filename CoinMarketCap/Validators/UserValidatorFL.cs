using CoinMarketCap.Dtos;
using CoinMarketCap.Helpers;
using FluentValidation;

namespace CoinMarketCap.Validators
{
    public class UserValidatorFL : AbstractValidator<UserDto>
    {
        public UserValidatorFL()
        {
            var comment = new Comment();
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Login).NotEmpty().WithMessage("InvalidUserName").MaximumLength(35);
                RuleFor(x => x.Password).NotEmpty().WithMessage("InvalidPassword").Length(6, 10)
                    .WithMessage("InvalidPasswordSize");

            });
        }
    }
}
