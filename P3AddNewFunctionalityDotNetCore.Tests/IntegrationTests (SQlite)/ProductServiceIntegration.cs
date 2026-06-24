using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using ProductServiceResources = P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.IntegrationTests
{

    public class ProductServiceIntegration
    {
        private DbContextOptions<P3Referential> _dbContextOptions;
        private P3Referential _dbContext;
        private ICart _cart; 
        private IProductRepository _productRepository;
        private IProductService _productService;
        private ProductController _productController;
        
        private OrderController _orderController;
        private IStringLocalizer<OrderController> _localizer;
        private IOrderService _orderService;


        public ProductServiceIntegration()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

            _dbContextOptions = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlite("Data Source=:memory:")
            .Options;

            _dbContext = new P3Referential(_dbContextOptions, null);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _cart = new Cart();
            _productRepository = new ProductRepository(_dbContext);
            _productService = new ProductService(_cart, _productRepository);
            _productController = new ProductController(_productService);

        }


        /// <summary>
        /// Integration tests for ProductService using SQLite in-memory database.
        /// Tests cover product add, delete and availability for customer view and cart.
        /// </summary>
        [Fact]
        public void SaveProduct_With_ProductController()
        {
            // ARRANGE
            ProductViewModel productAdd = new ProductViewModel { Name = "ABC", Stock = "2", Price = "10,50" };

            // ACT
            var result = _productController.Create(productAdd) as ViewResult;

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public void DeleteProduct_With_ProductController()
        {
            // ARRANGE
            ProductViewModel productAdd = new ProductViewModel { Name = "ABC", Stock = "2", Price = "10,50" };

            // ACT
            _productController.Create(productAdd);
            var adminResut = _productController.Index() as ViewResult;
            var productList = adminResut.Model as IEnumerable<ProductViewModel>;
            var lastProductId = productList.LastOrDefault().Id;
            _productController.DeleteProduct(lastProductId);

            // ASSERT
            Assert.DoesNotContain(_productRepository.GetAllProducts(), i => i.Id == lastProductId);
        }

        [Fact]
        public void AdminView_NewProduct_With_ProductController()
        {
            // ARRANGE
            ProductViewModel productAdd = new ProductViewModel { Name = "ABC", Stock = "2", Price = "10,50" };

            // ACT
            _productController.Create(productAdd);
            var adminResut = _productController.Admin() as ViewResult;
            var productList = adminResut.Model as IEnumerable<ProductViewModel>;
            var productFind = productList.LastOrDefault();

            // ASSERT
            Assert.Equal("ABC", productFind.Name);
            Assert.Equal("2", productFind.Stock);
            Assert.Equal("10,50", productFind.Price);
        }

        [Fact]
        public void UserView_NewProduct_With_ProductController()
        {
            // ARRANGE
            ProductViewModel productAdd = new ProductViewModel { Name = "ABC", Stock = "2", Price = "10,50" };

            // ACT
            _productController.Create(productAdd);
            var userResut = _productController.Index() as ViewResult;
            var productList = userResut.Model as IEnumerable<ProductViewModel>;
            var productFind = productList.LastOrDefault();

            // ASSERT
            Assert.Equal("ABC", productFind.Name);
            Assert.Equal("2", productFind.Stock);
            Assert.Equal("10,50", productFind.Price);
        }

        [Fact]
        public void AddToCart_NewProduct_With_CartController()
        {
            // ARRANGE
            CartController _cartController = new CartController(_cart, _productService);
            ProductViewModel productAdd = new ProductViewModel { Name = "ABC", Stock = "2", Price = "10,50" };

            // ACT
            _productController.Create(productAdd);
            var userResut = _productController.Index() as ViewResult;
            var productList = userResut.Model as IEnumerable<ProductViewModel>;
            var lastProductId = productList.LastOrDefault().Id;
            _cartController.AddToCart(lastProductId);
            var lastCartLine = _cart.Lines.LastOrDefault();


            // ASSERT
            Assert.Equal(lastProductId, lastCartLine.Product.Id);
            Assert.Equal("ABC", lastCartLine.Product.Name);
            Assert.Equal(10.50, lastCartLine.Product.Price);
        }

        [Fact]
        public void FinalizeOrder_NewProduct_With_OrderController()
        {
            // ARRANGE
            var mockLocalizer = new Mock<IStringLocalizer<OrderController>>();

            CartController _cartController = new CartController(_cart, _productService);
            IOrderRepository _orderRepository = new OrderRepository(_dbContext);
            IOrderService _orderService = new OrderService(_cart, _orderRepository , _productService);
            OrderController _orderController = new OrderController(_cart, _orderService, mockLocalizer.Object, _productService);
            ProductViewModel productAdd = new ProductViewModel { Name = "ABC", Stock = "2", Price = "10,50" };

            // ACT
            _productController.Create(productAdd);
            var userResut = _productController.Index() as ViewResult;
            var productList = userResut.Model as IEnumerable<ProductViewModel>;
            var lastProductId = productList.LastOrDefault().Id;
            _cartController.AddToCart(lastProductId);
            OrderViewModel order = new OrderViewModel
            {
                Name = "NameTest",
                Address = "RueTest",
                City = "VilleTest",
                Zip = "01234",
                Country = "PaysTest"
            };
            _orderController.Index(order);
            var orders = _orderService.GetOrders().Result;
            var lastOrder = orders.LastOrDefault();


            // ASSERT
            Assert.Contains(lastOrder.OrderLine, i => i.ProductId == lastProductId);

        }



    }
}