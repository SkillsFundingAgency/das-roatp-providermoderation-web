﻿using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;
using SFA.DAS.Roatp.ProviderModeration.Web.Validators;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Validators
{
    [TestFixture]
    public class ProviderDescriptionReviewModelValidatorTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void WhenEmpty_ProducesValidatonError(string providerDescription)
        {
            var sut = new ProviderDescriptionReviewModelValidator();

            var submitModel = new ProviderDescriptionReviewViewModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldHaveValidationErrorFor(c => c.ProviderDescription).WithErrorMessage(ProviderDescriptionSubmitModelValidator.ProviderDescriptionEmptyMessage);
        }

        [Test]
        public void WhenTooLong_ProducesValidatonError()
        {
            string providerDescription = new string('*', 751);
            var sut = new ProviderDescriptionReviewModelValidator();

            var submitModel = new ProviderDescriptionReviewViewModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldHaveValidationErrorFor(c => c.ProviderDescription).WithErrorMessage(ProviderDescriptionSubmitModelValidator.ProviderDescriptionLengthErrorMessage);
        }

        [Test]
        public void WhenValid_ShouldNotHaveErrorForEmail()
        {
            string providerDescription = new string('*', 750);
            var sut = new ProviderDescriptionSubmitModelValidator();

            var submitModel = new ProviderDescriptionSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldNotHaveValidationErrorFor(c => c.ProviderDescription);
        }
    }
}
