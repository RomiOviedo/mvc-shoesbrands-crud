using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMVC.Entidades;
using ShoppingMVC.Servicios.Interfaces;
using ShoppingMVC.Web.ViewModels.Brand;
using ShoppingMVC.Web.ViewModels.Shoe;
using X.PagedList.Extensions;

namespace ShoppingMVC.Web.Controllers
{
    [Authorize] // solo usuarios registrados pueden ingresar

    public class ShoeController : Controller
    {
        private readonly IServicioShoe _service;
        private readonly IMapper _mapper;
        private readonly IServicioBrand _brandService;

        public ShoeController(IServicioShoe service, IMapper mapper, IServicioBrand brandService)
        {
            _service = service;
            _mapper = mapper;
            _brandService = brandService;
        }
        public IActionResult Index(int? page, string? search, int? brandId)
        {
            int pageNumber = page ?? 1;
            int pageSize = 6;

            ViewBag.ResetUrl = Url.Action("Index");

            ViewBag.Brands = _brandService.GetAll()
                .Select(b => new SelectListItem  // using Rendering representa una opcion de un select
                {
                    Text = b.BrandName,
                    Value = b.BrandId.ToString()

                });


            //var shoes = _service.GetAll(filtro:!string.IsNullOrEmpty(search)
            //    ? s=>s.ShoeName.Contains(search.ToLower()) || s.Brand.BrandName.Contains(search.ToLower())
            //    :null,
            //    orderBy: o => o.OrderBy(s => s.ShoeName), propertiesNames: "Brand"); 


            var shoes = _service.GetAll(
                       filtro: s =>
                           (string.IsNullOrEmpty(search)
                               || s.ShoeName.Contains(search)
                               || s.Brand.BrandName.Contains(search))
                           &&
                           (!brandId.HasValue || s.BrandId == brandId),
                       orderBy: o => o.OrderBy(s => s.ShoeName),
                       propertiesNames: "Brand"
                        );


           var shoesVM = _mapper.Map<List<ShoeListVM>>(shoes).ToPagedList(pageNumber, pageSize);

            var vm = new ShoeIndexVM()
            {
                Shoes= shoesVM,
                Search=search,
                BrandId=brandId,
                Brands= _brandService.GetAll()  
                .Select(b=> new SelectListItem()
                {
                    Text=b.BrandName,
                    Value=b.BrandId.ToString()

                })

            };
            return View(vm);
        }

        [Authorize(Roles ="Admin")] // registrados mas rol admin
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id is null)
            {
                return NotFound();
            }

            Shoe? shoe = _service?.Get(filter: f => f.ShoeId == id);

            if (shoe is null)
            {
                return NotFound();
            }
            _service?.Delete(shoe);

            return Json(new { success = true, message = "Record deleted successfully" });

        }

        [Authorize(Roles = "Admin")]

        public IActionResult UpSert(int? id)
        {
            ShoeEditVM shoeEditVM;

            if (id == 0 || id is null)
            {
                shoeEditVM = new ShoeEditVM();
                RecargarCombos(shoeEditVM);
            }
            else
            {
                try
                {
                    Shoe? shoe = _service?.Get(s => s.ShoeId == id);

                    if (shoe is null)
                    {
                        return NotFound();
                    }

                    shoeEditVM = _mapper.Map<ShoeEditVM>(shoe);
                    RecargarCombos(shoeEditVM);


                    return View(shoeEditVM);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(shoeEditVM);

        }

        private void RecargarCombos(ShoeEditVM? shoeEditVM)
        {
            shoeEditVM!.Brands = _brandService.GetAll()!
                .Select(b => new SelectListItem()
                {
                    Text = b.BrandName,
                    Value = b.BrandId.ToString()
                }).ToList();
        }

        [HttpPost]  // accion despues de confirmar
        public IActionResult UpSert(ShoeEditVM? shoeEditVM)
        {
            if (!ModelState.IsValid)
            {
                RecargarCombos(shoeEditVM);

                return View(shoeEditVM);
            }
            try
            {
                Shoe? shoe = _mapper?.Map<Shoe>(shoeEditVM);

                if (shoe == null)
                {
                    ModelState.AddModelError(string.Empty, " no shoe has been supplied");

                    return View(shoeEditVM);

                }
                if (_service?.Existe(shoe) ?? true)  // ?? puede ser null, por defecto es true
                {
                    ModelState.AddModelError(string.Empty, " duplicate record / registro duplicado!! ");

                    return View(shoeEditVM);
                }

                _service.Guardar(shoe);

                TempData["success"] = "record added/edited successfully !";


                return RedirectToAction("Index"); // vuelve a la vista index, x q  da error de no todas las rutas devuelven un valor
            }

            catch (Exception)
            {
                RecargarCombos(shoeEditVM);

                ModelState.AddModelError(string.Empty, "an error occurred while editing the record");

                return View(shoeEditVM);
            }
        }


    }
}
