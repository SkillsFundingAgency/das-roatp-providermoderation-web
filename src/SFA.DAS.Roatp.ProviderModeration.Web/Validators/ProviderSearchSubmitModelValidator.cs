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
                .Must(BeAValidInt).WithMessage(InvalidUkprnErrorMessage)
                .Must(BeAValidUkprn).WithMessage(InvalidUkprnErrorMessage);
        }
        private bool BeAValidInt(string ukprnInput)
        {
            return int.TryParse(ukprnInput.ToString(), out _);
        }
        private bool BeAValidUkprn(string ukprnInput)
        {
            return int.Parse(ukprnInput.ToString()) > 10000000 && int.Parse(ukprnInput.ToString()) < 99999999;
        }
    }
}
