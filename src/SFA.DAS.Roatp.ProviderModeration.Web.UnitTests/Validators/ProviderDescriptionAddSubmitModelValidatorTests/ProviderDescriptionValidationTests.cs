using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.Roatp.ProviderModeration.Web.Models;
using SFA.DAS.Roatp.ProviderModeration.Web.Validators;

namespace SFA.DAS.Roatp.ProviderModeration.Web.UnitTests.Validators.ProviderDescriptionAddSubmitModelValidatorTests
{
    [TestFixture]
    public class ProviderDescriptionValidationTests
    {
        public const string ProviderDescriptionValidatesTo750Characters = "ABC Solutions is a national provider offering a range of apprenticeships across a variety of industry sectors including digital, IT, HR, finance, business, teaching, early years & sport.\r\nWe are the number 1 provider within the school sector & have worked with large MATs for many years, to recruit new talent, upskill staff & create bespoke career pathways.  LMP offers 20 plus courses and a 'one stop shop' for MATs and schools.\r\nABC is graded 'Good' by OFSTED, ranked 3rd best provider by apprentices through 'Rate My Apprenticeship' & has sector leading Qualification Achievement Rates - 77.4% for Teaching Assistant, 98% for the Library, Information & Archive Assistant & 83.2% for School Business Professional. Our Qualification Pass Rate is 89.9%.";
       
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void WhenEmpty_ProducesValidatonError(string providerDescription)
        {
            var sut = new ProviderDescriptionSubmitModelValidator();

            var submitModel = new ProviderDescriptionSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldHaveValidationErrorFor(c => c.ProviderDescription).WithErrorMessage(ProviderDescriptionSubmitModelValidator.ProviderDescriptionEmptyMessage);
        }

        [Test]
        public void WhenTooLong_ProducesValidatonError()
        {
            string providerDescription = ProviderDescriptionValidatesTo750Characters + "x";
            var sut = new ProviderDescriptionSubmitModelValidator();

            var submitModel = new ProviderDescriptionSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldHaveValidationErrorFor(c => c.ProviderDescription).WithErrorMessage(ProviderDescriptionSubmitModelValidator.ProviderDescriptionLengthErrorMessage);
        }

        [Test]
        public void WhenValid_ShouldNotHaveErrorForProviderDescription()
        { 
            var providerDescription = ProviderDescriptionValidatesTo750Characters;
            var sut = new ProviderDescriptionSubmitModelValidator();

            var submitModel = new ProviderDescriptionSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            providerDescription.Length.Should().BeGreaterThan(750);
            result.ShouldNotHaveValidationErrorFor(c => c.ProviderDescription);
        }

        [Test]
        public void WhenValid_ShouldNotHaveErrorForProviderDescriptionWhenCarriageReturnAdded()
        {
            string providerDescription = ProviderDescriptionValidatesTo750Characters + "\r";
            var sut = new ProviderDescriptionSubmitModelValidator();

            var submitModel = new ProviderDescriptionSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldNotHaveValidationErrorFor(c => c.ProviderDescription);
        }

        [Test]
        public void WhenValid_ShouldNotHaveErrorForProviderDescriptionWhenLineFeedAdded()
        {
            string providerDescription = ProviderDescriptionValidatesTo750Characters + "\n";
            var sut = new ProviderDescriptionSubmitModelValidator();

            var submitModel = new ProviderDescriptionSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldNotHaveValidationErrorFor(c => c.ProviderDescription);
        }


        [TestCase("@")]
        [TestCase("#")]
        [TestCase("$")]
        [TestCase("^")]
        [TestCase("=")]
        [TestCase("+")]
        [TestCase("/")]
        [TestCase("\\")]
        [TestCase("<")]
        [TestCase(">")]
        public void WhenInValidCharacter_ShouldHaveErrorForProviderDescription(string providerDescription)
        {
            var sut = new ProviderDescriptionSubmitModelValidator();

            var submitModel = new ProviderDescriptionSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldHaveValidationErrorFor(c => c.ProviderDescription).WithErrorMessage(ProviderDescriptionSubmitModelValidator.ProviderDescriptionHasInvalidCharacter); ;
        }

        [TestCase("a")]
        [TestCase("A")]
        [TestCase("0")]
        [TestCase("9")]
        [TestCase("-")]
        [TestCase("a b")]
        [TestCase("\"")]
        [TestCase("\'")]
        [TestCase(",")]
        [TestCase(".")]
        public void WhenValidCharacters_ShouldHaveNoErrorForProviderDescription(string providerDescription)
        {
            var sut = new ProviderDescriptionSubmitModelValidator();

            var submitModel = new ProviderDescriptionSubmitModel()
            {
                ProviderDescription = providerDescription
            };

            var result = sut.TestValidate(submitModel);

            result.ShouldNotHaveAnyValidationErrors();        }
    }
}
