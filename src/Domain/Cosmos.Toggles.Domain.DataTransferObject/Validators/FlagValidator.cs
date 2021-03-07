using FluentValidation;

namespace Cosmos.Toggles.Domain.DataTransferObject.Validators
{
    public class FlagValidator : AbstractValidator<Flag>
    {
        public FlagValidator()
        {
            RuleFor(x => x.Environment).NotNull();
            RuleFor(x => x.Environment.Project).NotNull();
            RuleFor(x => x.Environment.Project.Id).NotNull().NotEmpty();
            RuleFor(x => x.Environment.Id).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
        }
    }
}
