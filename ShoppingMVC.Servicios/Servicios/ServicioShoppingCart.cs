using ShoppingMVC.Datos;
using ShoppingMVC.Datos.Interfaces;
using ShoppingMVC.Entidades;
using ShoppingMVC.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Servicios.Servicios
{
    public class ServicioShoppingCart:IServicioShoppingCart
    {
        private readonly IRepositorioShoppingCart _repo;
        private readonly IUnitOfWork _unitOfWork;

        public ServicioShoppingCart(IRepositorioShoppingCart repo, IUnitOfWork unitOfWork)
        {
            _repo = repo ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(ShoppingCart shoppingCart)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repo.Delete(shoppingCart);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {

                _unitOfWork.Rollback();
            }
        }

        public ShoppingCart? Get(Expression<Func<ShoppingCart, bool>>? filter = null, string? propertiesName = null, bool tracked = true)
        {
            return _repo.Get(filter, propertiesName, tracked);
        }

        public IEnumerable<ShoppingCart>? GetAll(Expression<Func<ShoppingCart, bool>>? filter = null, Func<IQueryable<ShoppingCart>, IOrderedQueryable<ShoppingCart>>? orderBy = null, string propertiesNames = null)
        {
            return _repo.GetAll(filter, orderBy, propertiesNames);
        }

        public void Save(ShoppingCart shoppingCart)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (shoppingCart.ShoppingCartId==0)
                {
                    _repo.Add(shoppingCart);

                }
                else
                {
                    _repo.Update(shoppingCart);   
                }
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
