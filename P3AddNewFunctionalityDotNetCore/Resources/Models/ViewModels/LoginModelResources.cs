using System.Resources;
using System.Reflection;
using System.Globalization;


namespace P3AddNewFunctionalityDotNetCore.Resources.Models.ViewModels
{
    public class LoginModelResources
    {
        private static ResourceManager _resourceManager = new ResourceManager("P3AddNewFunctionalityDotNetCore.Resources.Models.ViewModels.LoginModel", Assembly.GetExecutingAssembly());
        private static CultureInfo _resourceCulture;

        public static string ErrorMissingName
        {
            get
            {
                return _resourceManager.GetString("ErrorMissingName", _resourceCulture);
            }
        }
        public static string ErrorMissingPassword
        {
            get
            {
                return _resourceManager.GetString("ErrorMissingPassword", _resourceCulture);
            }
        }
        
    }
}
