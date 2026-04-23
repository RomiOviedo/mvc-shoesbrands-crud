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
    public class ServicioOrderDetail : IServicioOrderDetail
    {
        private readonly IRepositorioOrderDetail _repo;
        private readonly IUnitOfWork _unitOfWork;

        public ServicioOrderDetail(IRepositorioOrderDetail repo, IUnitOfWork unitOfWork)
        {
            _repo = repo ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }


        public void Delete(OrderDetail OrderDetail)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repo.Delete(OrderDetail);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {

                _unitOfWork.Rollback();
            }

        }

        public OrderDetail? Get(Expression<Func<OrderDetail, bool>>? filter = null, string? propertiesName = null, bool tracked = true)
        {
            return _repo.Get(filter, propertiesName, tracked);

        }

        public IEnumerable<OrderDetail>? GetAll(Expression<Func<OrderDetail, bool>>? filter = null, Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>>? orderBy = null, string propertiesNames = null)
        {
            return _repo.GetAll(filter, orderBy, propertiesNames);

        }

        public void Save(OrderDetail OrderDetail)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                if (OrderDetail.OrderDetailId == 0)
                {
                    _repo.Add(OrderDetail);

                }
                else
                {
                    _repo.UpDate(OrderDetail);
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
