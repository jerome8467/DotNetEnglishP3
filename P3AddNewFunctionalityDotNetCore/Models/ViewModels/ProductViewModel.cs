using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3AddNewFunctionalityDotNetCore.Attributes;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {

        [BindNever]
        public int Id { get; set; }


        // INPUT NAME WITH DATA ANNOTATIONS
        [Required(ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = "MissingName")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Details { get; set; }

        // INPUT STOCK WITH DATA ANNOTATIONS
        [Required(ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = "MissingStock")]
        [StockValidation]
        public string Stock { get; set; }

        // INPUT PRICE WITH DATA ANNOTATIONS
        [Required(ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = "MissingPrice")]
        [PriceValidation]
        public string Price { get; set; }


    }
}
