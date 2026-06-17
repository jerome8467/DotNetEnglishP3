using System.ComponentModel.DataAnnotations;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;


namespace P3AddNewFunctionalityDotNetCore.Attributes
{
    public class StockValidation: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString())) 
                return new ValidationResult(ProductService.MissingStock, new[] { "Stock" });

            if (!int.TryParse(value.ToString(), out int stock))
                return new ValidationResult(ProductService.StockNotAnInteger, new[] { "Stock" });

            if (stock <= 0)
                return new ValidationResult(ProductService.StockNotGreaterThanZero, new[] { "Stock" });


            return ValidationResult.Success;
        }
    }

}
