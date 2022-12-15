using FluentValidation;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Validators
{
    public class ProviderDescriptionAddSubmitModelValidator : AbstractValidator<ProviderDescriptionAddSubmitModel>
    {
        public const string ProviderDescriptionEmptyMessage = "Please provide a description";
        public const string ProviderDescriptionLengthErrorMessage = "Provide description Maximum 750 characters";
        public ProviderDescriptionAddSubmitModelValidator()
        {
            RuleFor(x => x.ProviderDescription)
                .NotEmpty().WithMessage(ProviderDescriptionEmptyMessage)
                .MaximumLength(750).WithMessage(ProviderDescriptionLengthErrorMessage);
        }
    }
}
