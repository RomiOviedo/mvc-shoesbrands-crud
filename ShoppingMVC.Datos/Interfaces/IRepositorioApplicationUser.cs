using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Datos.Interfaces
{
    public interface IRepositorioApplicationUser:IRepositorioGenerico<ApplicationUser>
    {
        public void UpDate(ApplicationUser applicationUser);
    }
}
