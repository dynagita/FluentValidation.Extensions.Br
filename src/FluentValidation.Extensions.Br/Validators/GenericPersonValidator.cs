namespace FluentValidation.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class GenericPersonValidator<T, TProperty> : PropertyValidator<T, TProperty>, IBrazilianPropertyValidator
    {
        private readonly string _errorMessage;

        protected abstract int ValidLength { get; }
        protected abstract int[] FirstMultiplierCollection { get; }
        protected abstract int[] SecondMultiplierCollection { get; }

        protected GenericPersonValidator(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => string.IsNullOrWhiteSpace(_errorMessage) ? base.GetDefaultMessageTemplate(errorCode) : _errorMessage;

        protected abstract string Sanitize(string value);

        protected virtual int[] GetNumericValues(string sanitized)
            => sanitized.Select(x => (int)char.GetNumericValue(x)).ToArray();

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (EqualityComparer<TProperty>.Default.Equals(value, default)) return false;

            var val = Sanitize(value as string ?? string.Empty);

            if (val.Length != ValidLength || AllDigitsAreEqual(val))
                return false;

            var numbers = GetNumericValues(val);

            return val.EndsWith(GetCheckDigits(numbers));
        }

        private static bool AllDigitsAreEqual(string value)
            => value.Distinct().Count() == 1;

        private string GetCheckDigits(int[] numbers)
        {
            var first = CalculateValue(FirstMultiplierCollection, numbers);
            var second = CalculateValue(SecondMultiplierCollection, numbers);
            return $"{CalculateDigit(first)}{CalculateDigit(second)}";
        }

        private static int CalculateValue(int[] weights, int[] numbers)
        {
            var sum = 0;
            for (int i = 0; i < weights.Length; i++)
                sum += weights[i] * numbers[i];
            return sum;
        }

        private static int CalculateDigit(int sum)
        {
            var mod = sum % 11;
            return mod < 2 ? 0 : 11 - mod;
        }
    }
}
