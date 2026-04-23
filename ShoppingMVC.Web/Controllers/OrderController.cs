using AspNetCoreGeneratedDocument;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.Servicios.Interfaces;
using System.Security.Claims;

namespace ShoppingMVC.Web.Controllers
{
    public class OrderController:Controller
    {
        private readonly IServicioOrderHeader _servicioOrder;
        private readonly IServicioShoe _servicioShoe;
        private readonly IMapper _mapper;

        public OrderController(IServicioOrderHeader servicioOrder, IServicioShoe servicioShoe, IMapper mapper)
        {
            _servicioOrder = servicioOrder;
            _servicioShoe = servicioShoe;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var orderHeader = _servicioOrder.Get(filter: o => o.OrderHeaderId == id, propertiesName: "OrderDetail");
            foreach (var detail in orderHeader!.OrderDetail)
            {
                var shoeInDetail = _servicioShoe.Get(filter: s => s.ShoeId == detail.ShoeId, propertiesNames: "Shoe");
                detail.Shoe= shoeInDetail;
            }

            return View(orderHeader);
        }


        #region API Calls
        [HttpGet]
        public JsonResult GetAll()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;

            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var orderList= _servicioOrder.GetAll(filter:
                o=>o.ApplicationUserId== claims!.Value);
            return Json(new { data = orderList });
        }
        #endregion

    }
}
