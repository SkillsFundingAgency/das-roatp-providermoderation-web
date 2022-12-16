using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;
using SFA.DAS.Roatp.ProviderModeration.Web.Validators;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Validators.ProviderDescriptionAddSubmitModelValidatorTests
{
    [TestFixture]
    public class ValidateProviderDescriptionTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void WhenEmpty_ProducesValidatonError(string providerDescription)
        {
            var sut = new ProviderDescriptionAddSubmitModelValidator();

            var submitModel = new ProviderDescriptionAddSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldHaveValidationErrorFor(c => c.ProviderDescription).WithErrorMessage(ProviderDescriptionAddSubmitModelValidator.ProviderDescriptionEmptyMessage);
        }

        [Test]
        public void WhenTooLong_ProducesValidatonError()
        {
            string providerDescription = new string('*', 751);
            var sut = new ProviderDescriptionAddSubmitModelValidator();

            var submitModel = new ProviderDescriptionAddSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldHaveValidationErrorFor(c => c.ProviderDescription).WithErrorMessage(ProviderDescriptionAddSubmitModelValidator.ProviderDescriptionLengthErrorMessage);
        }

        [Test]
        public void WhenValid_ShouldNotHaveErrorForEmail()
        {
            string providerDescription = new string('*', 750);
            var sut = new ProviderDescriptionAddSubmitModelValidator();

            var submitModel = new ProviderDescriptionAddSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldNotHaveValidationErrorFor(c => c.ProviderDescription);
        }
    }
}
