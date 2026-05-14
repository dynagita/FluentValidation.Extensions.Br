using FluentValidation;

namespace FluentValidator.Extensions.Br.Tests.Api.Validators
{
    public class CpfCnpjValidator : AbstractValidator<string>
    {
        public CpfCnpjValidator()
        {
            RuleFor(x => x)
                .IsValidCpfOrCnpj()
                .WithMessage("Documento inválido.");
        }
    }
}
