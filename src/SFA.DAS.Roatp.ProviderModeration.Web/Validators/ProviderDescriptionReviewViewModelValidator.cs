using FluentValidation;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Validators
{
    public class ProviderDescriptionReviewViewModelValidator : AbstractValidator<ProviderDescriptionReviewViewModel>
    {
        public ProviderDescriptionReviewViewModelValidator()
        {
            Include(new ProviderDescriptionSubmitModelValidator());
        }
    }
}
