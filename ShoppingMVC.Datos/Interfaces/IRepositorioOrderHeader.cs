using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Datos.Interfaces
{
    public interface IRepositorioOrderHeader:IRepositorioGenerico<OrderHeader>
    {
        public void UpDate(OrderHeader orderHeader);
    }
}
