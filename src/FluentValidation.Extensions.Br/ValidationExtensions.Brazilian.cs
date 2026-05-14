namespace FluentValidation
{
    using Validators;

    /// <summary>
    /// A partial class with brazilian extension methods for validations
    /// </summary>
    public static partial class DefaultValidatorExtensions
    {
        /// <summary>
        /// Defines a 'CNPJ' validator on the current rule builder.
        /// Accepts both the current numeric format and the new alphanumeric format (Receita Federal, July 2026).
        /// Validation will fail if the property is null, empty, or the value is an invalid CNPJ.
        /// </summary>
        public static IRuleBuilderOptions<T, string> IsValidCNPJ<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new CNPJValidator<T, string>());
        }

        /// <summary>
        /// Defines a 'CPF' validator on the current rule builder.
        /// Validation will fail if the property is null, empty, or the value is an invalid CPF.
        /// </summary>
        public static IRuleBuilderOptions<T, string> IsValidCPF<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new CpfValidator<T, string>());
        }

        /// <summary>
        /// Defines a 'CPF or CNPJ' validator on the current rule builder.
        /// Accepts CPF (11 digits) or CNPJ in any format (numeric or alphanumeric, 14 characters).
        /// Validation will fail if the property is null, empty, or neither a valid CPF nor a valid CNPJ.
        /// </summary>
        public static IRuleBuilderOptions<T, string> IsValidCpfOrCnpj<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new CPFCNPJValidator<T, string>());
        }
    }
}
