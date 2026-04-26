using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.Entidades;
using ShoppingMVC.Servicios.Interfaces;
using ShoppingMVC.Web.ViewModels.Order;
using ShoppingMVC.Web.ViewModels.ShopCart;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace ShoppingMVC.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController:Controller
    {
        private readonly IServicioShoppingCart _serviceCart;
        private readonly IServicioApplicationUser _serviceUser;
        private readonly IServicioShoe _serviceShoe;
        private readonly IServicioOrderHeader _serviceOrderHeader;
        private readonly IMapper _mapper;

        public ShoppingCartController(IServicioShoppingCart serviceCart, IServicioApplicationUser serviceUser, IServicioShoe serviceShoe, IMapper mapper, IServicioOrderHeader serviceOrderHeader)
        {
            _serviceCart = serviceCart?? throw new ApplicationException("Dependencies not set");
            _serviceUser= serviceUser ?? throw new ApplicationException("Dependencies not set");
            _serviceShoe = serviceShoe ?? throw new ApplicationException("Dependencies not set"); 
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set"); 
            _serviceOrderHeader= serviceOrderHeader ?? throw new ApplicationException("Dependencies not set");

        }

        public IActionResult Index(string? returnUrl = null)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            if (userId == null)
            {
                return View();
            }

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

        public IActionResult Add(int shoeid, string? returnUrl=null)
        {
            ClaimsIdentity claimsIdenttity = (ClaimsIdentity)User.Identity!;

            var userId= claimsIdenttity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var carInDb = _serviceCart.Get(
                filter: c => c.ApplicationUserId ==  userId && c.ShoeId==shoeid);


            if (carInDb != null) // ya existe este shoe agregado al carrito del user
            {
                carInDb.Quantity += 1; // sumo un producto
                _serviceCart.Save(carInDb);  // guardo cambios
            }
            else
            {
                ShoppingCart cart = new ShoppingCart()  // carrito(item de ese shoe seleccionado) no existe, creo uno nuevo 
                {
                    ApplicationUserId = userId,
                    ShoeId = shoeid,
                    Quantity = 1
                };
                _serviceCart.Save(cart);
            }
            return RedirectToAction("Index", new { returnUrl = returnUrl });

        }

        public IActionResult Minus(int shoeId)
        {
            ClaimsIdentity claimsIdenttity = (ClaimsIdentity)User.Identity!;

            var userId = claimsIdenttity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var carInDb = _serviceCart.Get(
                filter: c => c.ApplicationUserId == userId && c.ShoeId == shoeId);


            
                carInDb!.Quantity -= 1; // resto un producto
                _serviceCart.Save(carInDb);  // guardo cambios

            if (carInDb.Quantity == 0)
            {
                _serviceCart.Delete(carInDb);
            }
            else
            {
                _serviceCart.Save(carInDb);
            }

                return RedirectToAction("Index");


        }

        public IActionResult Remove(int shoeId)
        {
            ClaimsIdentity claimsIdenttity = (ClaimsIdentity)User.Identity!;

            var userId = claimsIdenttity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var carInDb = _serviceCart.Get(
                filter: c => c.ApplicationUserId == userId && c.ShoeId == shoeId);

            _serviceCart.Delete(carInDb!);

            return RedirectToAction("Index");
        }
        

        public IActionResult Summary(string? returnUrl = null)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // busca el id del usuario actual
            var user = _serviceUser.Get(filter: u => u.Id == userId);   // busco el usuario en mi bd, para trabajar con sus datos ej: nombre, apellido, direccion, ...  

            var cartList = _serviceCart.GetAll(
                c => c.ApplicationUserId == userId! ,  // traigo todos los carritos de este user
                propertiesNames:"Shoe")!.ToList();     // traigo los shoe para mostrar en la vista de sumary                                  

            ShoppingCartListVm shoppingVM = new ShoppingCartListVm
            {
                ShoppingCarts = cartList,
                OrderHeader = new OrderHeaderEditVM()
                {
                    OrderTotal = CalculateTotal(cartList),
                    OrderDate = DateTime.Now,
                    ShippingDate = DateTime.Now.AddDays(4),
                    OrderDetail = _mapper.Map<List<OrderDetail>>(cartList),  // para hacer el orderdetail necesito el join con shoe
                    ApplicationUserId = user!.Id,
                    FirstName = user.FirstName!,
                    LastName=user.LastName!,
                    Address=user.Addres!

                }
            };

            ViewBag.ReturnUrl = returnUrl;

            return View(shoppingVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(ShoppingCartListVm shoppingVm)
        {
            Debug.WriteLine($"ModelState válido? {ModelState.IsValid}");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartList = _serviceCart.GetAll(
                filter: c => c.ApplicationUserId == userId, propertiesNames: "Shoe")!.ToList();
            var user = _serviceUser.Get(filter: u => u.Id == userId);   // busco el usuario en mi bd, para trabajar con sus datos ej: nombre, apellido, direccion, ...  

            shoppingVm.OrderHeader!.OrderTotal = CalculateTotal(cartList);
            shoppingVm.OrderHeader.OrderDate = DateTime.Now;
            shoppingVm.OrderHeader.ShippingDate= DateTime.Now.AddDays(4);
            shoppingVm.OrderHeader.OrderDetail = _mapper.Map<List<OrderDetail>>(cartList);
            shoppingVm.OrderHeader.ApplicationUserId = userId!;
            shoppingVm.OrderHeader.FirstName = user!.FirstName!;
            shoppingVm.OrderHeader.LastName = user.LastName!;
            shoppingVm.OrderHeader.Address = user.Addres!;

            for (int i = 0; i < cartList.Count(); i++)
            {
                shoppingVm.OrderHeader.OrderDetail[i].ShoeId = cartList[i].ShoeId;
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
                return View(shoppingVm);
            }

            OrderHeader orderHeader = _mapper.Map<OrderHeader>(shoppingVm.OrderHeader);
            try
            {
                _serviceOrderHeader.Save(orderHeader);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(shoppingVm);
            }

            return RedirectToAction("OrderConfirmed", new { id = orderHeader.OrderHeaderId }); // new{...} crea un objeto anonino
        }                                                                                       // pasa parametros a la URL
                                                                                                // se combierte en /OrderConfirmed?id=5     
        
        public IActionResult OrderConfirmed(int id)
        {
            return View(id);
        }
        
        
        
        public IActionResult OrderCancelled(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
    }
}
