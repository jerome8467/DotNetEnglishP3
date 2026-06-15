using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Globalization;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.UnitTests
{
    public class ProductViewModelTest
    {
        public ProductViewModelTest()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
        }
        private string validateName;
        private string validateStock;
        private string validatePrice;

        /// <summary>
        /// Validates the ProductViewModel and stores the result for each field.
        /// </summary>
        public void ValidateProductModel(ProductViewModel model)
        {
            validateName = null;
            validateStock = null;
            validatePrice = null;

            var errors = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, errors, true);

            var nameError = errors.FirstOrDefault(n => n.MemberNames.Contains("Name"));
            if (nameError != null){ validateName = nameError.ErrorMessage; }
            else { validateName = "NameValid"; }

            var stockError = errors.FirstOrDefault(n => n.MemberNames.Contains("Stock"));
            if (stockError != null) { validateStock = stockError.ErrorMessage; }
            else { validateStock = "StockValid"; }

            var priceError = errors.FirstOrDefault(n => n.MemberNames.Contains("Price"));
            if (priceError != null) { validatePrice = priceError.ErrorMessage; }
            else { validatePrice = "PriceValid"; }
        }

        [Fact]
        public void AddProduct_With_NameEmpty() 
        {
            // ARRANGE
            ProductViewModel model = new ProductViewModel { Name = null, Stock = "1", Price = 1 };

            // ACT
            ValidateProductModel(model);

            // ASSERT
            Assert.Equal("Veuillez saisir un nom", validateName);
            Assert.Equal("StockValid", validateStock);
            Assert.Equal("PriceValid", validatePrice);

        }

        [Fact]
        public void AddProduct_With_StockeEmpty()
        {
            // ARRANGE
            ProductViewModel model = new ProductViewModel { Name = "test", Stock = null, Price = 1 };

            // ACT
            ValidateProductModel(model);

            // ASSERT
            Assert.Equal("NameValid", validateName);
            Assert.Equal("Veuillez saisir un stock", validateStock);
            Assert.Equal("PriceValid", validatePrice);

        }

        [Fact]
        public void AddProduct_With_StockZero()
        {
            // ARRANGE
            ProductViewModel model = new ProductViewModel { Name = "test", Stock = "0", Price = 1 };

            // ACT
            ValidateProductModel(model);

            // ASSERT
            Assert.Equal("NameValid", validateName);
            Assert.Equal("Le stock doit être supérieure à zéro", validateStock);
            Assert.Equal("PriceValid", validatePrice);

        }

        [Fact]
        public void AddProduct_With_StockDouble()
        {
            // ARRANGE
            ProductViewModel model = new ProductViewModel { Name = "test", Stock = "2.5", Price = 1 };

            // ACT
            ValidateProductModel(model);

            // ASSERT
            Assert.Equal("NameValid", validateName);
            Assert.Equal("La valeur saisie pour le stock doit être un entier", validateStock);
            Assert.Equal("PriceValid", validatePrice);

        }

        [Fact]
        public void AddProduct_With_StockNegatif()
        {
            // ARRANGE
            ProductViewModel model = new ProductViewModel { Name = "test", Stock = "-2", Price = 1 };

            // ACT
            ValidateProductModel(model);

            // ASSERT
            Assert.Equal("NameValid", validateName);
            Assert.Equal("Le stock doit être supérieure à zéro", validateStock);
            Assert.Equal("PriceValid", validatePrice);

        }

        [Fact]
        public void AddProduct_With_PriceEmpty()
        {
            // ARRANGE
            ProductViewModel model = new ProductViewModel { Name = "test", Stock = "1", Price = null };

            // ACT
            ValidateProductModel(model);

            // ASSERT
            Assert.Equal("NameValid", validateName);
            Assert.Equal("StockValid", validateStock);
            Assert.Equal("Veuillez saisir un prix", validatePrice);

        }

        [Fact]
        public void AddProduct_With_PriceZero()
        {
            // ARRANGE
            ProductViewModel model = new ProductViewModel { Name = "test", Stock = "1", Price = 0 };

            // ACT
            ValidateProductModel(model);

            // ASSERT
            Assert.Equal("NameValid", validateName);
            Assert.Equal("StockValid", validateStock);
            Assert.Equal("Le prix doit être supérieur à zéro", validatePrice);

        }

        [Fact]
        public void AddProduct_With_PriceNegatif()
        {
            // ARRANGE
            ProductViewModel model = new ProductViewModel { Name = "test", Stock = "1", Price = -5 };

            // ACT
            ValidateProductModel(model);

            // ASSERT
            Assert.Equal("NameValid", validateName);
            Assert.Equal("StockValid", validateStock);
            Assert.Equal("Le prix doit être supérieur à zéro", validatePrice);

        }

        [Fact]
        public void AddProduct_With_AllValid()
        {
            // ARRANGE
            ProductViewModel model = new ProductViewModel { Name = "test", Stock = "1", Price = 1 };

            // ACT
            ValidateProductModel(model);

            // ASSERT
            Assert.Equal("NameValid", validateName);
            Assert.Equal("StockValid", validateStock);
            Assert.Equal("PriceValid", validatePrice);

        }

    }
}
