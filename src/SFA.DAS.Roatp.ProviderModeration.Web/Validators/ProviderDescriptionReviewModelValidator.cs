using FluentValidation;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Validators
{
    public class ProviderDescriptionReviewModelValidator : AbstractValidator<ProviderDescriptionReviewViewModel>
    {
        public ProviderDescriptionReviewModelValidator()
        {
            Include(new ProviderDescriptionSubmitModelValidator());
        }
    }
}
