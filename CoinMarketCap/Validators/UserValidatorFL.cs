using CoinMarketCap.Dtos;
using CoinMarketCap.Helpers;
using CoinMarketCap.Models.Enums;
using FluentValidation;

namespace CoinMarketCap.Validators
{
    public class UserValidatorFL : AbstractValidator<UserDto>
    {
        private readonly Comment _comment;
        public UserValidatorFL(Comment comment)
        {
            _comment = comment;
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Login).NotEmpty().WithMessage(_comment.InvalidRegisLogin).MaximumLength(35);
                RuleFor(x => x.Password).NotEmpty().WithMessage(_comment.InvalidRegisPassword).Length(6, 10)
                    .WithMessage(_comment.InvalidRegisPassword);

            });
        }
    }
}
