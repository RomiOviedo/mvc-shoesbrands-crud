using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Datos.Interfaces
{
    public interface IRepositorioOrderDetail:IRepositorioGenerico<OrderDetail>
    {
        void UpDate(OrderDetail orderDetail);
    }
}
