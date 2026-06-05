using System;
using System.Resources;
using System.Reflection;
using System.Globalization;


namespace P3AddNewFunctionalityDotNetCore.Resources.Models.ViewModels
{
    public class LoginModelResources
    {
        private static ResourceManager resourceManager = new ResourceManager("P3AddNewFunctionalityDotNetCore.Resources.Models.ViewModels.LoginModel", Assembly.GetExecutingAssembly());
        private static CultureInfo resourceCulture;

        public static string ErrorMissingName
        {
            get
            {
                return resourceManager.GetString("ErrorMissingName", resourceCulture);
            }
        }
        public static string ErrorMissingPassword
        {
            get
            {
                return resourceManager.GetString("ErrorMissingPassword", resourceCulture);
            }
        }
        
    }
}
