using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.UnitTests
{
    
    public class LoginModelTest
    {
        private const string ValidAdminName = "Admin";
        private const string ValidAdminPassword = "P@ssword123";

        private string validateName;
        private string validatePassword;

        private void ValidateLoginModel(LoginModel model)
        {
            validateName = "";
            validatePassword = "";
            var errors = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, errors, true);
            if (errors.Any(n => n.MemberNames.Contains("Name"))) { validateName = "NameWrong"; }
            else { validateName = "NameValid"; }
            if (errors.Any(p => p.MemberNames.Contains("Password"))) { validatePassword = "PasswordWrong"; }
            else { validatePassword = "PasswordValid"; }
        }


        [Fact]
        public void Login_With_AllWrong()
        {
            // Arrange
            LoginModel model = new LoginModel{ Name = "", Password = ""};

            // Act
            ValidateLoginModel(model);

            // Assert
            Assert.Equal("NameWrong", validateName);
            Assert.Equal("PasswordWrong", validatePassword);

        }

        [Fact]
        public void Login_With_PasswordWrong()
        {
            // Arrange
            LoginModel model = new LoginModel { Name = ValidAdminName, Password = "" };

            // Act
            ValidateLoginModel(model);

            // Assert
            Assert.Equal("NameValid", validateName);
            Assert.Equal("PasswordWrong", validatePassword);

        }

        [Fact]
        public void Login_With_NameWrong()
        {
            // Arrange
            LoginModel model = new LoginModel { Name = "", Password = ValidAdminPassword };

            // Act
            ValidateLoginModel(model);

            // Assert
            Assert.Equal("NameWrong", validateName);
            Assert.Equal("PasswordValid", validatePassword);

        }

        [Fact]
        public void Login_With_AllValid()
        {
            // Arrange
            LoginModel model = new LoginModel { Name = ValidAdminName, Password = ValidAdminPassword };

            // Act
            ValidateLoginModel(model);

            // Assert
            Assert.Equal("NameValid", validateName);
            Assert.Equal("PasswordValid", validatePassword);

        }
    }
}
