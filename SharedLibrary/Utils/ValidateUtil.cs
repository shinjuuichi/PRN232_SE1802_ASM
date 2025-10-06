using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SharedLibrary.Utils
{
    public static class ValidateUtil
    {
        public static void TryValidate(this object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (validationResults.Count != 0)
            {
                Dictionary<string, string> errors = [];

                foreach (var validationResult in validationResults)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        errors[memberName] = validationResult.ErrorMessage ?? "Invalid value";
                    }
                }

                throw new Exception(JsonSerializer.Serialize(errors));
            }
        }
    }
}