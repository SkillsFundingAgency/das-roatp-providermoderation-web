using FluentValidation;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;
using System.Text.RegularExpressions;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Validators
{
    public class ProviderSearchSubmitModelValidator : AbstractValidator<ProviderSearchSubmitModel>
    {
        public const string UKPRNEmptyMessage = "You must enter an UKPRN";
        public const string UKPRNInvalidMessage = "Enter a valid UKPRN";
        public const string UKPRNRegex = @"^\d{8}$";
        public ProviderSearchSubmitModelValidator()
        {
            RuleFor(m => m.UKPRN)
                .NotEmpty()
                .WithMessage(UKPRNEmptyMessage);
        }
    }
}
