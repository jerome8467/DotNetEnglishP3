using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.IntegrationTests
{

    public class ProductServiceTests
    {
        private DbContextOptions<P3Referential> _dbContextOptions;
        private P3Referential _dbContext;
        private ICart _cart; 
        private IProductRepository _productRepository;
        private IProductService _productService;


        public ProductServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlite("Data Source=:memory:")
            .Options;

            _dbContext = new P3Referential(_dbContextOptions, null);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _cart = new Cart();
            _productRepository = new ProductRepository(_dbContext);
            _productService = new ProductService(_cart, _productRepository);
        }

        private static Product MapToProductEntity(string name, double price, int quantity, string description, string details)
        {
            Product productEntity = new Product
            {
                Name = name,
                Price = price,
                Quantity = quantity,
                Description = description,
                Details = details
            };
            return productEntity;
        }

        /// <summary>
        /// Integration tests for ProductService using SQLite in-memory database.
        /// Tests cover product add, delete and availability for customer view and cart.
        /// </summary>

        [Fact]
        public void Add_Product()
        {
            // Arrange
            var newProduct = MapToProductEntity("Test produit", 49.99, 10, "Test Description", "Test Détails");
            _productRepository.SaveProduct(newProduct);

            // Act
            List<Product> listProduct = _productService.GetAllProducts();

            // Assert
            Assert.Equal("Test produit", listProduct.LastOrDefault()?.Name);
            Assert.Equal(49.99, listProduct.LastOrDefault()?.Price);
            Assert.Equal(10, listProduct.LastOrDefault()?.Quantity);
            Assert.Equal("Test Description", listProduct.LastOrDefault()?.Description);
            Assert.Equal("Test Détails", listProduct.LastOrDefault()?.Details);
        }

        [Fact]
        public void Delete_Product()
        {
            // Arrange
            var newProduct = MapToProductEntity("Test produit", 49.99, 10, "Test Description", "Test Détails");
            _productRepository.SaveProduct(newProduct);
            string result;

            // Act
            List<Product> listProduct = _productService.GetAllProducts();
            int idLastProduct = listProduct.Last().Id;
            _productService.DeleteProduct(idLastProduct);
            List<Product> updateList = _productService.GetAllProducts();
            if (updateList.Count > 0)
            {
                int newIdLastProduct = updateList.LastOrDefault().Id;
                result = (idLastProduct == newIdLastProduct) ? "Produit non supprimé" : "Produit supprimé";
            }
            else
            {
                result = "Produit supprimé";
            }

            // Assert
            Assert.Equal("Produit supprimé", result);
        }

        [Fact]
        public void New_Product_In_Customer_View()
        {
            // Arrange
            var newProduct = MapToProductEntity("Test produit", 49.99, 10, "Test Description", "Test Détails");
            _productRepository.SaveProduct(newProduct);


            // Act
            IEnumerable<Product> listProduct = _productRepository.GetAllProducts();


            // Assert
            Assert.Equal("Test produit", listProduct.LastOrDefault()?.Name);
            Assert.Equal(49.99, listProduct.LastOrDefault()?.Price);
            Assert.Equal(10, listProduct.LastOrDefault()?.Quantity);
            Assert.Equal("Test Description", listProduct.LastOrDefault()?.Description);
            Assert.Equal("Test Détails", listProduct.LastOrDefault()?.Details);
        }

        [Fact]
        public void New_Product_In_Cart()
        {
            // Arrange
            var newProduct = MapToProductEntity("Test produit", 49.99, 10, "Test Description", "Test Détails");
            _productRepository.SaveProduct(newProduct);


            // Act
            List<Product> listProduct = _productService.GetAllProducts();
            int idProduct = listProduct.LastOrDefault().Id;


            _cart.AddItem(listProduct.LastOrDefault(), 2);
            CartLine lastCartLine = _cart.Lines.LastOrDefault();


            // Assert
            //Compare id Product
            Assert.Equal(idProduct, lastCartLine.Product.Id);
            //Compare Product
            Assert.Equal(newProduct.Name, lastCartLine.Product.Name);
            Assert.Equal(newProduct.Price, lastCartLine.Product.Price);
            Assert.Equal(newProduct.Description, lastCartLine.Product.Description);
            Assert.Equal(newProduct.Details, lastCartLine.Product.Details);

        }
    }
}