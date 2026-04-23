using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShoppingMVC.Servicios.Interfaces
{
    public interface IServicioShoppingCart
    {
        IEnumerable<ShoppingCart>? GetAll(
            Expression<Func<ShoppingCart, bool>>? filter = null,
            Func<IQueryable<ShoppingCart>,IOrderedQueryable<ShoppingCart>>? orderBy=null,
            string? propertiesNames= null);


        void Save(ShoppingCart shoppingCart);

        void Delete(ShoppingCart shoppingCart);

        ShoppingCart? Get(
            Expression<Func<ShoppingCart, bool>>? filter = null,
            string? propertiesName = null,
            bool tracked = true);





    }
}
