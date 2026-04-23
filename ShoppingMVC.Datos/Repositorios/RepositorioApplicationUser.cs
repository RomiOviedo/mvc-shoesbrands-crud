using ShoppingMVC.Datos.Interfaces;
using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Datos.Repositorios
{
    public class RepositorioApplicationUser : RepositorioGenerico<ApplicationUser>, IRepositorioApplicationUser
    {
        private readonly ShoppingMvcDbContext _db;
        public RepositorioApplicationUser(ShoppingMvcDbContext db):base(db)
        {
            _db = db;
        }
        public void UpDate(ApplicationUser applicationUser)
        {
            _db.ApplicationUsers.Update(applicationUser);
        }
    }
}
