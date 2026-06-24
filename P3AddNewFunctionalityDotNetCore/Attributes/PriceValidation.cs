using System;
using System.ComponentModel.DataAnnotations;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;


namespace P3AddNewFunctionalityDotNetCore.Attributes
{
    public class PriceValidation: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (!double.TryParse(value?.ToString(), out double price))
                return new ValidationResult(ProductService.PriceNotANumber, new[] { "Price" });

            if (Convert.ToDouble(price) <= 0)
                return new ValidationResult(ProductService.PriceNotGreaterThanZero, new[] { "Price" });


            return ValidationResult.Success;
        }
    }

}
