using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3AddNewFunctionalityDotNetCore.Resources.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }


        /// INPUT NAME WITH DATA ANNOTATIONS
        [Required(ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = "MissingName")]
        public string Name { get; set; }


        public string Description { get; set; }
        public string Details { get; set; }


        /// INPUT STOCK WITH DATA ANNOTATIONS
        [Required(ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = "MissingStock")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = "StockNotGreaterThanZero")]
        public int? Stock { get; set; }


        /// INPUT PRICE WITH DATA ANNOTATIONS
        [Required(ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = "MissingPrice")]
        [Range(1, double.PositiveInfinity, ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        public double? Price { get; set; }
    }
}
