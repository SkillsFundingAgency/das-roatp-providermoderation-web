using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;
using SFA.DAS.Roatp.ProviderModeration.Web.Validators;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Validators.ProviderSearchSubmitModelValidatorTests
{
    [TestFixture]
    public class ValidateUkprnTests
    {
        [TestCase(null)]
        public void WhenUkprnEmpty_ProducesValidatonError(string ukprn)
        {
            var sut = new ProviderSearchSubmitModelValidator();

            var model = new ProviderSearchSubmitModel()
            {
                Ukprn = ukprn,
            };

            var result = sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.Ukprn)
                  .WithErrorMessage(ProviderSearchSubmitModelValidator.UkprnEmptyMessage);
        }

        [TestCase("10000000")]
        [TestCase("100000000")]
        public void WhenUkprnOutOfRange_ProducesValidatonError(string ukprn)
        {
            var sut = new ProviderSearchSubmitModelValidator();

            var model = new ProviderSearchSubmitModel()
            {
                Ukprn = ukprn,
            };

            var result = sut.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.Ukprn)
                  .WithErrorMessage(ProviderSearchSubmitModelValidator.InvalidUkprnErrorMessage);
        }

        [TestCase("99999998")]
        [TestCase("12345678")]
        public void WhenUkprnWithInRange_ProducesValidatonNoError(string ukprn)
        {
            var sut = new ProviderSearchSubmitModelValidator();

            var model = new ProviderSearchSubmitModel()
            {
                Ukprn = ukprn,
            };

            var result = sut.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(c => c.Ukprn);
        }
    }
}
