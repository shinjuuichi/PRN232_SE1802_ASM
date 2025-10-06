using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.CustomAttributes
{
    public class NotContainsOnlineOrPartTimeAttribute : ValidationAttribute
    {
        public NotContainsOnlineOrPartTimeAttribute()
        {
            ErrorMessage = "The Title cannot contain these word 'Online' or 'Part-time'.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str)
            {
                if (str.Contains("Online", StringComparison.OrdinalIgnoreCase) || str.Contains("Part-time", StringComparison.OrdinalIgnoreCase))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
