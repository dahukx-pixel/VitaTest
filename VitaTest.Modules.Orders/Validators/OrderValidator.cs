using FluentValidation;

namespace VitaTest.Modules.Orders.Validators
{
    public class OrderValidator : AbstractValidator<decimal>
    {
        public OrderValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Не может быть пустым.")
                           .GreaterThan(0).WithMessage("Должно быть больше 0.");
        }
    }
}
