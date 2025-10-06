using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.CustomAttributes
{
    public class CantContainAttribute : ValidationAttribute
    {
        private string _word;
        public CantContainAttribute(string word)
        {
            _word = word;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string strValue && strValue.Contains(_word, StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult($"The field {validationContext.DisplayName} cannot contain the word '{_word}'.");
            }
            return ValidationResult.Success;
        }
    }
}
