namespace FluentValidation.Validators
{
    using System;

    public class CPFCNPJValidator<T, TProperty> : PropertyValidator<T, TProperty>, IBrazilianPropertyValidator
    {
        private readonly string _errorMessage;
        private readonly CpfValidator<T, TProperty> _cpf;
        private readonly CNPJValidator<T, TProperty> _cnpj;

        public CPFCNPJValidator(string errorMessage = "O CPF ou CNPJ é inválido!", DateTime? alphaStartDate = default)
        {
            _errorMessage = errorMessage;
            _cpf = new CpfValidator<T, TProperty>();
            _cnpj = new CNPJValidator<T, TProperty>(alphaStartDate: alphaStartDate);
        }

        public override string Name => "CpfCnpjValidator";

        protected override string GetDefaultMessageTemplate(string errorCode)
            => string.IsNullOrWhiteSpace(_errorMessage) ? base.GetDefaultMessageTemplate(errorCode) : _errorMessage;

        public override bool IsValid(ValidationContext<T> context, TProperty value)
            => _cpf.IsValid(context, value) || _cnpj.IsValid(context, value);
    }
}
