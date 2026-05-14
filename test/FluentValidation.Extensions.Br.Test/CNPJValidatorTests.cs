namespace FluentValidation.Extensions.Br.Test
{
    using System;
    using Results;
    using Validators;
    using Xunit;
    using System.Linq;

    public class CNPJValidatorTests
    {
        [Theory]
        [InlineData("11.434.325/0001-07")]  // numeric with mask
        [InlineData("11434325000107")]       // numeric without mask
        public void When_CNPJ_Is_Valid_Then_The_Validator_Should_Pass(string cnpj)
        {
            TestExtensionsValidator validator = new TestExtensionsValidator(x => x.RuleFor(r => r.CNPJ).IsValidCNPJ());
            ValidationResult result = validator.Validate(new Person { CNPJ = cnpj });

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("12.ABC.345/01DE-35")]  // alphanumeric with mask
        [InlineData("12ABC34501DE35")]       // alphanumeric without mask
        public void When_CNPJ_Is_Alphanumeric_And_AlphaStartDate_Has_Passed_Then_The_Validator_Should_Pass(string cnpj)
        {
            TestExtensionsValidator validator = new TestExtensionsValidator(
                x => x.RuleFor(r => r.CNPJ).SetValidator(new CNPJValidator<Person, string>(alphaStartDate: new DateTime(2000, 1, 1)))
            );
            ValidationResult result = validator.Validate(new Person { CNPJ = cnpj });

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("12.ABC.345/01DE-35")]  // alphanumeric with mask — rejected before alpha start date
        [InlineData("12ABC34501DE35")]       // alphanumeric without mask — rejected before alpha start date
        public void When_CNPJ_Is_Alphanumeric_Before_AlphaStartDate_Then_The_Validator_Should_Fail(string cnpj)
        {
            TestExtensionsValidator validator = new TestExtensionsValidator(x => x.RuleFor(r => r.CNPJ).IsValidCNPJ());
            ValidationResult result = validator.Validate(new Person { CNPJ = cnpj });

            Assert.False(result.IsValid);
            Assert.Equal("O CNPJ é inválido!", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void When_CNPJ_Is_Invalid_Then_Set_Custom_Message_And_Validator_Should_Fail()
        {
            const string customMessage = "Custom Message";

            TestExtensionsValidator validator = new TestExtensionsValidator(x => x.RuleFor(r => r.CNPJ).IsValidCNPJ().WithMessage(customMessage));
            ValidationResult result = validator.Validate(new Person { CNPJ = "00.000.000/0000-00" });

            string errorMessage = result.Errors.FirstOrDefault()?.ErrorMessage ?? string.Empty;

            Assert.False(result.IsValid);
            Assert.Equal(customMessage, errorMessage);
        }

        [Theory]
        [InlineData("14.442.344/1210-57")]
        [InlineData("11.434.3215/00123-57")]
        [InlineData("A11.434.325/0001-07")]
        [InlineData("11.434.325/0001-07a")]
        [InlineData("12.ABC.345/01DE-99")]   // alphanumeric with wrong check digits
        [InlineData("12.abc.345/01de-35")]   // lowercase letters are invalid per Receita Federal spec
        public void When_CNPJ_Is_Invalid_Then_The_Validator_Should_Fail(string cnpj)
        {
            TestExtensionsValidator validator = new TestExtensionsValidator(x => x.RuleFor(r => r.CNPJ).IsValidCNPJ());
            ValidationResult result = validator.Validate(new Person { CNPJ = cnpj });

            Assert.False(result.IsValid);
            Assert.Equal("O CNPJ é inválido!", result.Errors.First().ErrorMessage);
        }
    }
}
