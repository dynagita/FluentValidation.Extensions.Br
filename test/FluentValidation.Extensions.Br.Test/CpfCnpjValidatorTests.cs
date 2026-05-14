namespace FluentValidation.Extensions.Br.Test
{
    using System;
    using Results;
    using Validators;
    using Xunit;
    using System.Linq;

    public class CpfCnpjValidatorTests
    {
        [Theory]
        [InlineData("822.420.106-62")]       // CPF with mask
        [InlineData("82242010662")]           // CPF without mask
        [InlineData("11.434.325/0001-07")]   // numeric CNPJ with mask
        [InlineData("11434325000107")]        // numeric CNPJ without mask
        public void When_CpfOrCnpj_Is_Valid_Then_The_Validator_Should_Pass(string value)
        {
            TestExtensionsValidator validator = new TestExtensionsValidator(x => x.RuleFor(r => r.CpfOrCnpj).IsValidCpfOrCnpj());
            ValidationResult result = validator.Validate(new Person { CpfOrCnpj = value });

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("12.ABC.345/01DE-35")]   // alphanumeric CNPJ with mask
        [InlineData("12ABC34501DE35")]        // alphanumeric CNPJ without mask
        public void When_CpfOrCnpj_Is_Alphanumeric_Cnpj_And_AlphaStartDate_Has_Passed_Then_The_Validator_Should_Pass(string value)
        {
            TestExtensionsValidator validator = new TestExtensionsValidator(
                x => x.RuleFor(r => r.CpfOrCnpj).SetValidator(new CPFCNPJValidator<Person, string>(alphaStartDate: new DateTime(2000, 1, 1)))
            );
            ValidationResult result = validator.Validate(new Person { CpfOrCnpj = value });

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("12.ABC.345/01DE-35")]   // alphanumeric CNPJ with mask — rejected before alpha start date
        [InlineData("12ABC34501DE35")]        // alphanumeric CNPJ without mask — rejected before alpha start date
        public void When_CpfOrCnpj_Is_Alphanumeric_Cnpj_Before_AlphaStartDate_Then_The_Validator_Should_Fail(string value)
        {
            TestExtensionsValidator validator = new TestExtensionsValidator(x => x.RuleFor(r => r.CpfOrCnpj).IsValidCpfOrCnpj());
            ValidationResult result = validator.Validate(new Person { CpfOrCnpj = value });

            Assert.False(result.IsValid);
            Assert.Equal("O CPF ou CNPJ é inválido!", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void When_CpfOrCnpj_Is_Invalid_Then_Set_Custom_Message_And_Validator_Should_Fail()
        {
            const string customMessage = "Custom Message";

            TestExtensionsValidator validator = new TestExtensionsValidator(x => x.RuleFor(r => r.CpfOrCnpj).IsValidCpfOrCnpj().WithMessage(customMessage));
            ValidationResult result = validator.Validate(new Person { CpfOrCnpj = "000.000.000-00" });

            string errorMessage = result.Errors.FirstOrDefault()?.ErrorMessage ?? string.Empty;

            Assert.False(result.IsValid);
            Assert.Equal(customMessage, errorMessage);
        }

        [Theory]
        [InlineData("144.442.344-57")]
        [InlineData("14.442.344/1210-57")]
        [InlineData("12.ABC.345/01DE-99")]   // alphanumeric CNPJ with wrong check digits
        [InlineData("A822.420.106-62")]
        [InlineData("11.434.325/0001-07a")]
        [InlineData("000.000.000-00")]
        [InlineData("00.000.000/0000-00")]
        [InlineData(null)]
        [InlineData("")]
        public void When_CpfOrCnpj_Is_Invalid_Then_The_Validator_Should_Fail(string value)
        {
            TestExtensionsValidator validator = new TestExtensionsValidator(x => x.RuleFor(r => r.CpfOrCnpj).IsValidCpfOrCnpj());
            ValidationResult result = validator.Validate(new Person { CpfOrCnpj = value });

            Assert.False(result.IsValid);
            Assert.Equal("O CPF ou CNPJ é inválido!", result.Errors.First().ErrorMessage);
        }
    }
}
