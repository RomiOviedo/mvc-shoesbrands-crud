using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShoppingMVC.Entidades;
using ShoppingMVC.Servicios.Interfaces;
using ShoppingMVC.Web.ViewModels.ApplicationUser;
using X.PagedList.Extensions;

namespace ShoppingMVC.Web.Controllers
{
    public class ApplicationUserController:Controller
    {

        private readonly IServicioApplicationUser _userService;
        private readonly IMapper _mapper;

        public ApplicationUserController(IServicioApplicationUser? user, IMapper? mapper)
        {
            _userService = user?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper?? throw new ApplicationException("Dependencies not set");
                
        }

        public IActionResult Index(int? page, string? searchTerm = null, bool viewAll = false,
            int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.CurrentPageSizee = pageSize;

            IEnumerable<ApplicationUser>? users;

            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    users = _userService.GetAll(
                        orden: o => o.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
                        filter: u => u.FirstName.Contains(searchTerm) || u.LastName.Contains(searchTerm)

                        );
                }
                else
                {   
                    users = _userService.GetAll(orden: o => o.OrderBy(u => u.LastName).ThenBy(u => u.FirstName));
                }
            }
            else
            {
                users = _userService.GetAll(orden: o=>o.OrderBy(u=>u.LastName).ThenBy(u=>u.FirstName));
            }

            var usersVm = _mapper.Map<List<ApplicationUserListVM>>(users).ToPagedList(pageNumber, pageSize);

            return View(usersVm);
        }


    }
}
