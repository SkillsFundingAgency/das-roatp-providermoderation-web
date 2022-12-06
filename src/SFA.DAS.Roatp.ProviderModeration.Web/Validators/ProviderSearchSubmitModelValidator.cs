using FluentValidation;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Validators
{
    public class ProviderSearchSubmitModelValidator : AbstractValidator<ProviderSearchSubmitModel>
    {
        public const string UkprnEmptyMessage = "You must enter a valid UKPRN";
        public const string InvalidUkprnErrorMessage = "You have entered an invalid UKPRN";
        public ProviderSearchSubmitModelValidator()
        {
            RuleFor(x => x.Ukprn)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(UkprnEmptyMessage)
                .GreaterThan(10000000).WithMessage(InvalidUkprnErrorMessage)
                .LessThan(99999999).WithMessage(InvalidUkprnErrorMessage);
        }
    }
}
