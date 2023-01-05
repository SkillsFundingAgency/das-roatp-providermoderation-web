using FluentValidation;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Validators
{
    public class ProviderDescriptionAddSubmitModelValidator : AbstractValidator<ProviderDescriptionAddSubmitModel>
    {
        public const string ProviderDescriptionEmptyMessage = "Enter provider description";
        public const string ProviderDescriptionLengthErrorMessage = "Provider description must be 750 characters or less";
        public ProviderDescriptionAddSubmitModelValidator()
        {
            RuleFor(x => x.ProviderDescription)
                .NotEmpty().WithMessage(ProviderDescriptionEmptyMessage)
                .MaximumLength(750).WithMessage(ProviderDescriptionLengthErrorMessage);
        }
    }
}
