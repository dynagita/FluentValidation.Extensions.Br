using FluentValidation;
using System.Data;

namespace FluentValidator.Extensions.Br.Tests.Api.Validators
{
    public class CnpjValidator : AbstractValidator<string>
    {
        public CnpjValidator()
        {
            RuleFor(x => x)
                .IsValidCNPJ()
                .WithMessage("CNPJ Inválido.");
        }
    }
}
