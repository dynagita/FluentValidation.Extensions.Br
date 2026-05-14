namespace FluentValidation.Validators
{

    /// <summary>
    /// Ensures that the property value is a valid CNPJ number.
    /// </summary>
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class CNPJValidator<T, TProperty> : GenericPersonValidator<T, TProperty>
    {
        private readonly DateTime _alphaStartDate;

        public CNPJValidator(string errorMessage = "O CNPJ é inválido!", DateTime? alphaStartDate = null)
            : base(errorMessage)
        {
            _alphaStartDate = alphaStartDate ?? new DateTime(2026, 7, 1);
        }

        protected override int ValidLength => 14;
        protected override int[] FirstMultiplierCollection => new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        protected override int[] SecondMultiplierCollection => new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        public override string Name => "CNPJValidator";

        protected override string Sanitize(string value)
            => Regex.Replace(value, "[^A-Z0-9]", "", RegexOptions.None, TimeSpan.FromSeconds(1));

        // ASCII(char) - 48: digits keep face value (0-9), A=17, B=18 ... Z=42.
        protected override int[] GetNumericValues(string sanitized)
            => sanitized.Select(x => (int)x - 48).ToArray();

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (DateTime.Today < _alphaStartDate)
            {
                var raw = value as string ?? string.Empty;
                if (raw.Any(char.IsLetter))
                    return false;
            }

            return base.IsValid(context, value);
        }
    }
}
