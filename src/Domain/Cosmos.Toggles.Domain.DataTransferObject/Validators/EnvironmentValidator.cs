using FluentValidation;

namespace Cosmos.Toggles.Domain.DataTransferObject.Validators
{
    public class EnvironmentValidator : AbstractValidator<Environment>
    {
        public EnvironmentValidator()
        {
            RuleFor(x => x.Project).NotNull().DependentRules(() => {
                RuleFor(x => x.Project.Id).NotNull().NotEmpty();
            });
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
