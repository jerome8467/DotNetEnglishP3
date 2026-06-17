using System.ComponentModel.DataAnnotations;
using P3AddNewFunctionalityDotNetCore.Resources.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(LoginModelResources),
            ErrorMessageResourceName = "ErrorMissingName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(LoginModelResources),
            ErrorMessageResourceName = "ErrorMissingPassword")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}