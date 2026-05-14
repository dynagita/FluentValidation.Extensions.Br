using FluentValidation;

namespace FluentValidator.Extensions.Br.Tests.Api.Validators
{
    public class CpfValidator : AbstractValidator<string>
    {
        public CpfValidator()
        {
            RuleFor(x => x.Trim())
                .IsValidCPF()
                .WithMessage("CPF Inválido.");
        }
    }
}
