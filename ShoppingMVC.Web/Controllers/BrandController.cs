using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMVC.Entidades;
using ShoppingMVC.Servicios.Interfaces;
using ShoppingMVC.Web.ViewModels.Brand;
using System.ComponentModel.DataAnnotations;
using System.Net;
using X.PagedList.Extensions;


namespace ShoppingMVC.Web.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        //private readonly IWebHostEnvironment _webHostEnvironment; // entorno anfitrion -- para saber donde esta mi aplicacion en el servidor, la uso para guardar las imagenes
        private readonly IServicioBrand _service;
        private readonly IMapper? _mapper;

        public BrandController(IServicioBrand service, IMapper? mapper)
        {
            _service = service;
            _mapper = mapper;

        }


        public IActionResult Index(int? page, string? search)
        {

            int pageNumber = page ?? 1;
            int pageSize = 6;

           // ViewBag.search = search;
            ViewBag.ResetUrl = Url.Action("Index");


           //var brands = _service.GetAll()
           //     .Select(b => new SelectListItem  // using Rendering representa una opcion de un select
           //     {
           //         Text = b.BrandName,
           //         Value = b.BrandId.ToString()

           //     });


           
            var brands = _service.GetAll(
                filter: !string.IsNullOrEmpty(search)
                ? b => b.BrandName.ToLower().Contains(search.ToLower()) 
                : null,
              orderBy: o => o.OrderBy(b => b.BrandName)
              );


                var brandsVM = _mapper?
                .Map<List<BrandListVM>>(brands)
                .ToPagedList(pageNumber, pageSize);

            var vm = new BrandIndexVM()
            {
                Brands= brandsVM,
                Search=search
               
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            Brand? brand = _service?.Get(filter: b => b.BrandId == id);


            if (brand is null)
            {
                return NotFound();
            }

            try
            {
                if (_service== null || _mapper==null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependecncias no configuradas correctamnete");
                }

                if (_service.EstaRelacionado(brand.BrandId))
                {
                    return Json(new { success = false, message = "related record!, delete deny !" });
                }

                _service.Delete(brand);

                return Json(new { success = true, message = "record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "couldn't delete record !!" }); // no se pudo
            }
        }

        [Authorize(Roles = "Admin")]

        public IActionResult UpSert(int? id)
        {
            BrandEditVM brandEditVM;

            if (id is null || id == 0)
            {
                brandEditVM = new BrandEditVM();
            }
            else
            {
                try
                {
                    Brand? brandInBD = _service?.Get(filter: b => b.BrandId == id);

                    if (brandInBD is null)
                    {
                        return NotFound();
                    }

                    brandEditVM = _mapper?.Map<BrandEditVM>(brandInBD);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(brandEditVM);

        }



        [HttpPost]  //ACCION DESPUES DE CONFIRMAR
        public IActionResult UpSert(BrandEditVM brandEditVM)
        {
            if (!ModelState.IsValid)
            {
                return View(brandEditVM);
            }
            try
            {
                Brand? brand = _mapper?.Map<Brand>(brandEditVM);

                if (brand == null)
                {
                    ModelState.AddModelError(string.Empty, " no brand has been supplied");

                    return View(brandEditVM);

                }
                if (_service?.Existe(brand) ?? true)  // ?? puede ser null, por defecto es true
                {
                    ModelState.AddModelError(string.Empty, " duplicate record / registro duplicado!! ");

                    return View(brandEditVM);
                }

                _service.Save(brand);

                TempData["success"] = "record added/edited successfully !";


                return RedirectToAction("Index"); // vuelve a la vista index, da error de no todas las rutas devuelven un valor
            }

            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "an error occurred while editing the record");

                return View(brandEditVM);
            }

        }
    }
}
