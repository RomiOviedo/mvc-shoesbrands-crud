using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Servicios.Interfaces
{
    public interface IServicioOrderHeader
    {
        IEnumerable<OrderHeader>? GetAll(
Expression<Func<OrderHeader, bool>>? filter = null,
Func<IQueryable<OrderHeader>, IOrderedQueryable<OrderHeader>>? orderBy = null,
string? propertiesNames = null);


        void Save(OrderHeader OrderHeader);

        void Delete(OrderHeader OrderHeader);

        OrderHeader? Get(
            Expression<Func<OrderHeader, bool>>? filter = null,
            string? propertiesName = null,
            bool tracked = true);


    }
}
