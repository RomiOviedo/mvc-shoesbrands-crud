using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Servicios.Interfaces
{
    public interface IServicioOrderDetail
    {
        IEnumerable<OrderDetail>? GetAll(
    Expression<Func<OrderDetail, bool>>? filter = null,
    Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>>? orderBy = null,
    string? propertiesNames = null);


        void Save(OrderDetail OrderDetail);

        void Delete(OrderDetail OrderDetail);

        OrderDetail? Get(
            Expression<Func<OrderDetail, bool>>? filter = null,
            string? propertiesName = null,
            bool tracked = true);




    }
}
