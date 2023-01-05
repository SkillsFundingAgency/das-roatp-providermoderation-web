using FluentValidation;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Validators
{
    public class ProviderSearchSubmitModelValidator : AbstractValidator<ProviderSearchSubmitModel>
    {
        public const string UkprnEmptyMessage = "Enter a UKPRN";
        public const string InvalidUkprnErrorMessage = "Enter a UKPRN using 8 numbers";
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
