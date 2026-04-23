using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.Entidades;
using ShoppingMVC.Servicios.Interfaces;
using ShoppingMVC.Web.ViewModels.ShopCart;
using System.Security.Claims;

namespace ShoppingMVC.Web.Controllers
{
    public class ShoppingCartController:Controller
    {
        private readonly IServicioShoppingCart _serviceCart;
        private readonly IServicioApplicationUser _serviceUser;
        private readonly IServicioShoe _serviceShoe;
        private readonly IMapper _mapper;

        public ShoppingCartController(IServicioShoppingCart serviceCart, IServicioApplicationUser serviceUser, IServicioShoe serviceShoe, IMapper mapper)
        {
            _serviceCart = serviceCart?? throw new ApplicationException("Dependencies not set");
            _serviceUser= serviceUser ?? throw new ApplicationException("Dependencies not set");
            _serviceShoe = serviceShoe ?? throw new ApplicationException("Dependencies not set"); 
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set"); ;
                
        }

        public IActionResult Index(string? returnUrl = null)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var cartList = _serviceCart.GetAll(
                filter: c => c.ApplicationUserId == userId, propertiesNames: "Shoe")!.ToList();

            foreach (var item in cartList)
            {
                item.Shoe = _serviceShoe.Get(filter: s => s.ShoeId == item.ShoeId)!;
            }

            ShoppingCartListVm shoppingVM = new ShoppingCartListVm()
            {
                ShoppingCarts = cartList,
                OrderHeader = new ViewModels.Order.OrderHeaderEditVM()
                {
                    OrderTotal = CalculateTotal(cartList)
                }
            };
            ViewBag.ReturnUrl=returnUrl;
            return View(shoppingVM);
        }

        private decimal CalculateTotal(List<ShoppingCart> cartList)
        {
            var total = 0M;

            foreach (var item in cartList)
            {
                item.Shoe = _serviceShoe.Get(s => s.ShoeId == item.ShoeId)!;
                total += (item.Quantity == 1 ? item.Shoe.Price : item.Shoe.Price * 0.9M) * item.Quantity;
            }
            return total;
        }

        public IActionResult Plus(int id, string? returnUrl = null)
        {

        }
    }
}
