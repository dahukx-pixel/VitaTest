using FluentValidation;

namespace VitaTest.Modules.Settings.Validators
{
    public class DatabaseConnectionValidator : AbstractValidator<string>
    {
        public DatabaseConnectionValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Не может быть null.")
                           .NotEmpty().WithMessage("Не может быть пустым.")
                           .MinimumLength(2).WithMessage("Минимальная длина - 2.");
        }
    }
}
