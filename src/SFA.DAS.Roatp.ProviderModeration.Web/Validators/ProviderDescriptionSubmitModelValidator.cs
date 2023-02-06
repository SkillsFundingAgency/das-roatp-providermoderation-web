using FluentValidation;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Validators
{
    public class ProviderDescriptionSubmitModelValidator : AbstractValidator<ProviderDescriptionSubmitModel>
    {
        private const string ValidCharactersExpression = @"^[a-zA-Z0-9\u0080-\uFFA7?$@#()""'!,+\-=_:;.&€£*%\s\/\[\]]*$";
        public const string ProviderDescriptionHasInvalidCharacter = "Provider description has invalid character";
        public const string ProviderDescriptionEmptyMessage = "Enter provider description";
        public const string ProviderDescriptionLengthErrorMessage = "Provider description must be 750 characters or less";
        public ProviderDescriptionSubmitModelValidator()
        {
            RuleFor(x => x.ProviderDescription)
                .NotEmpty().WithMessage(ProviderDescriptionEmptyMessage)
                .MaximumLength(750).WithMessage(ProviderDescriptionLengthErrorMessage)
                .Matches(ValidCharactersExpression).WithMessage(ProviderDescriptionHasInvalidCharacter);
        }
    }
}
