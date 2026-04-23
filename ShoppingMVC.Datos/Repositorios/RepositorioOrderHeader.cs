using ShoppingMVC.Datos.Interfaces;
using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Datos.Repositorios
{
    public class RepositorioOrderHeader : RepositorioGenerico<OrderHeader>, IRepositorioOrderHeader
    {
        private readonly ShoppingMvcDbContext _db;

        public RepositorioOrderHeader(ShoppingMvcDbContext db):base(db)
        {
            _db = db ?? throw new ArgumentException(nameof(db));
        }

        public void UpDate(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }
    }
}
