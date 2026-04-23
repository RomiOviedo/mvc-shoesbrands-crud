using ShoppingMVC.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Servicios.Interfaces
{
    public interface IServicioApplicationUser
    {
        IEnumerable<ApplicationUser>? GetAll(
            Expression<Func<ApplicationUser, bool>>? filter = null,
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>>? orden = null,
            string? propertiesName = null);

        void Save(ApplicationUser applicationUser);

        void Delete(ApplicationUser application);

        ApplicationUser? Get(
            Expression<Func<ApplicationUser, bool>> filter = null,
            string? propertiesName=null,
            bool tracked = true
            );

    }
}
