using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models;
using ProductService = P3AddNewFunctionalityDotNetCore.Models.Services.ProductService;
using ProductServiceResources = P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;
using Moq;


namespace P3AddNewFunctionalityDotNetCore.Tests.UnitTests
{
    public class ProductServiceTests
    {

        [Fact]
        public void AddProduct_With_NameEmpty()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = null, Stock = "5", Price = "10,50" };

            // ACT
            List <ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains("Name") 
                && e.ErrorMessage == ProductServiceResources.MissingName);
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Stock"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Price"));
        }

        [Fact]
        public void AddProduct_With_StockEmpty()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = "ABC", Stock = null, Price = "10,50" };

            // ACT
            List<ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains("Stock") 
                && e.ErrorMessage == ProductServiceResources.MissingStock);
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Name"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Price"));
        }

        [Fact]
        public void AddProduct_With_StockZero()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = "ABC", Stock = "0", Price = "10,50" };

            // ACT
            List<ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains("Stock") 
                && e.ErrorMessage == ProductServiceResources.StockNotGreaterThanZero);
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Name"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Price"));
        }

        [Fact]
        public void AddProduct_With_StockNegatif()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = "ABC", Stock = "-5", Price = "10,50" };

            // ACT
            List<ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains("Stock") 
                && e.ErrorMessage == ProductServiceResources.StockNotGreaterThanZero);
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Name"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Price"));
        }

        [Fact]
        public void AddProduct_With_StockDouble()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = "ABC", Stock = "2,5", Price = "10,50" };

            // ACT
            List<ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains("Stock") 
                && e.ErrorMessage == ProductServiceResources.StockNotAnInteger);
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Name"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Price"));
        }

        [Fact]
        public void AddProduct_With_PriceEmpty()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = "ABC", Stock = "5", Price = null };

            // ACT
            List<ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains("Price") 
                && e.ErrorMessage == ProductServiceResources.MissingPrice);
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Name"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Stock"));
        }

        [Fact]
        public void AddProduct_With_PriceZero()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = "ABC", Stock = "5", Price = "0" };

            // ACT
            List<ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains("Price") 
                && e.ErrorMessage == ProductServiceResources.PriceNotGreaterThanZero);
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Name"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Stock"));
        }

        [Fact]
        public void AddProduct_With_PriceNegatif()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = "ABC", Stock = "5", Price = "-5" };

            // ACT
            List<ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains("Price") 
                && e.ErrorMessage == ProductServiceResources.PriceNotGreaterThanZero);
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Name"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Stock"));
        }

        [Fact]
        public void AddProduct_With_AllValid()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = "ABC", Stock = "5", Price = "5" };

            // ACT
            List<ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.Empty(errors);
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Name"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Stock"));
            Assert.DoesNotContain(errors, e => e.MemberNames.Contains("Price"));
        }

        [Fact]
        public void AddProduct_With_AllEmpty()
        {
            // ARRANGE
            var mockRepository = new Mock<IProductRepository>();
            var mockCart = new Mock<ICart>();
            var productService = new ProductService(mockCart.Object, mockRepository.Object);
            var product = new ProductViewModel { Name = null, Stock = null, Price = null };

            // ACT
            List<ValidationResult> errors = productService.SaveProduct(product);

            // ASSERT
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.MemberNames.Contains("Name") 
                && e.ErrorMessage == ProductServiceResources.MissingName);
            Assert.Contains(errors, e => e.MemberNames.Contains("Stock") 
                && e.ErrorMessage == ProductServiceResources.MissingStock);
            Assert.Contains(errors, e => e.MemberNames.Contains("Price") 
                && e.ErrorMessage == ProductServiceResources.MissingPrice);
        }

    }
}
