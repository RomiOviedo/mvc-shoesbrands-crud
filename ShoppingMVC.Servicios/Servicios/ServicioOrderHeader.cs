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
    public class ServicioOrderHeader: IServicioOrderHeader
    {
        private readonly IRepositorioOrderHeader _repo;
        private readonly IRepositorioShoe _repoShoe;
        private readonly IRepositorioShoppingCart _repoShoppingCart;

        private readonly IUnitOfWork _unitOfWork;

        public ServicioOrderHeader(IRepositorioOrderHeader repo, IUnitOfWork unitOfWork, IRepositorioShoe repoShoe, IRepositorioShoppingCart repoShoppingCart)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
            _repo = repo ?? throw new ArgumentException("Dependencies not set");
            _repoShoe = repoShoe ?? throw new ArgumentException("Dependencies not set");
          _repoShoppingCart = repoShoppingCart ?? throw new ArgumentException("Dependencies not set");

        }

        public void Delete(OrderHeader OrderHeader)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repo.Delete(OrderHeader);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {

                _unitOfWork.Rollback();
            }

        }

        public OrderHeader? Get(Expression<Func<OrderHeader, bool>>? filter = null, string? propertiesName = null, bool tracked = true)
        {
            return _repo.Get(filter, propertiesName, tracked);

        }

        public IEnumerable<OrderHeader>? GetAll(Expression<Func<OrderHeader, bool>>? filter = null, Func<IQueryable<OrderHeader>, IOrderedQueryable<OrderHeader>>? orderBy = null, string? propertiesNames = null)
        {
            return _repo.GetAll(filter, orderBy, propertiesNames);
        }

        public void Save(OrderHeader OrderHeader)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (OrderHeader.OrderHeaderId == 0)
                {
                    _repo.Add(OrderHeader);

                    foreach (var item in OrderHeader.OrderDetail)
                    {
                      

                        var shoppingCart = _repoShoppingCart.Get(
                            filter: sc => sc.ShoeId == item.ShoeId 
                            && sc.ApplicationUserId == OrderHeader.ApplicationUserId);

                        _repoShoppingCart.Delete(shoppingCart); // borra item q contiene el id del producto especificado

                    }

                }
                else
                {
                    _repo.UpDate(OrderHeader);
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

