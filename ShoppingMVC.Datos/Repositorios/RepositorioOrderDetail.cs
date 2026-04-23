using ShoppingMVC.Datos.Interfaces;
using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Datos.Repositorios
{
    public class RepositorioOrderDetail :RepositorioGenerico<OrderDetail>, IRepositorioOrderDetail
    {
        private readonly ShoppingMvcDbContext _db;

        public RepositorioOrderDetail(ShoppingMvcDbContext db):base(db)
        {
                _db=db?? throw new ArgumentException(nameof(db));
        }

        public void UpDate(OrderDetail orderDetail)
        {
            _db.OrderDetails.Update(orderDetail);
        }
    }
}
