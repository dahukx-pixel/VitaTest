using FluentValidation;
using VitaTest.AppCore;

namespace VitaTest.Modules.Settings.Validators
{
    public class SettingsValidator : AbstractValidator<AppSettings>
    {
        public SettingsValidator()
        {
            var dbConnectionValidator = new DatabaseConnectionValidator();

            RuleFor(x => x.DbAddress).SetValidator(dbConnectionValidator);
            RuleFor(x => x.DbName).SetValidator(dbConnectionValidator);

            When(x => !x.LocalDb, () => RuleFor(x => x.DbLogin).SetValidator(dbConnectionValidator));
            When(x => !x.LocalDb, () => RuleFor(x => x.DbPassword).SetValidator(dbConnectionValidator));
        }
    }
}
