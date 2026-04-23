using ShoppingMVC.Datos.Interfaces;
using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Datos.Repositorios
{
    public class RepositorioShoppingCart : RepositorioGenerico<ShoppingCart>, IRepositorioShoppingCart
    {
        private readonly ShoppingMvcDbContext _db;

        public RepositorioShoppingCart(ShoppingMvcDbContext db):base(db)
        {
         _db=db ?? throw new ArgumentException(nameof(db));       
        }
        public void Update(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Update(shoppingCart);
        }
    }
}
