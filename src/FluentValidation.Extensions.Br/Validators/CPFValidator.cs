namespace FluentValidation.Validators
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class CpfValidator<T, TProperty> : GenericPersonValidator<T, TProperty>
    {
        public CpfValidator(string errorMessage = "O CPF é inválido!")
            : base(errorMessage) { }

        protected override int ValidLength => 11;
        protected override int[] FirstMultiplierCollection => new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        protected override int[] SecondMultiplierCollection => new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        public override string Name => "CPFValidator";

        protected override string Sanitize(string value)
        {
            if (value.Any(char.IsLetter))
                return string.Empty;

            return Regex.Replace(value, "[^0-9]", "", RegexOptions.None, TimeSpan.FromSeconds(1));
        }
    }
}
