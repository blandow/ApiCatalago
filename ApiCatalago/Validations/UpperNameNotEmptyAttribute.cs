using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.Validations
{
    public class UpperNameNotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("o Texto não pode ser nulo ou vazio");
            }
            if (!string.Equals(value.ToString()[0].ToString(),value.ToString()[0].ToString().ToUpper()))
            {
                return new ValidationResult("O nome deve iniciar com letra maiúscula");
            }
           
            
            return ValidationResult.Success;
        }
    }
}
