using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models.Entities;

namespace P3AddNewFunctionalityDotNetCore.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICart _cart;
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<OrderController> _localizer;
        private readonly IProductService _productService;

        public OrderController(ICart cart, IOrderService service, IStringLocalizer<OrderController> localizer, IProductService productservice)
        {
            _cart = cart;
            _orderService = service;
            _localizer = localizer;
            _productService = productservice;
        }

        public ViewResult Index()
        {
            return View(new OrderViewModel());
        }

        [HttpPost]
        public IActionResult Index(OrderViewModel order)
        {
            if (!((Cart) _cart).Lines.Any())
            {
                ModelState.AddModelError("", _localizer["CartEmpty"]);
            }
            if (ModelState.IsValid)
            {
                order.Lines = ((Cart) _cart)?.Lines.ToArray();
                foreach (CartLine line in order.Lines)
                {
                    Product product = _productService.GetProductById(line.Product.Id);
                    if (product != null) {
                        if (line.Quantity > product.Quantity)
                        {
                            ModelState.AddModelError("", _localizer["StockChange"]);
                            break;
                        }
                    }
                    else
                    {
                        _cart.RemoveLine(line.Product);
                        ModelState.AddModelError("", _localizer["ProductDelete"]);
                        break;
                    }
                }

                if (ModelState.IsValid)
                {
                    _orderService.SaveOrder(order);
                    return RedirectToAction(nameof(Completed));
                }
                else { return View(order); }
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            _cart.Clear();
            return View();
        }
    }
}
