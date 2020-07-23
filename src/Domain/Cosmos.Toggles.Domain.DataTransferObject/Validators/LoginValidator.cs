using FluentValidation;

namespace Cosmos.Toggles.Domain.DataTransferObject.Validators
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            });
        }
    }
}
