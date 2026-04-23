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
    public class ServicioAplicationUser : IServicioApplicationUser
    {
        private readonly IRepositorioApplicationUser _repo;
        private readonly IUnitOfWork _unitOfWork;


        public ServicioAplicationUser(IRepositorioApplicationUser repo, IUnitOfWork unitOfWork)
        {
            _repo = repo ?? throw new ArgumentException("dependencies not set");
            _unitOfWork = unitOfWork?? throw new ArgumentException("dependencies not set");
        }

        public void Delete(ApplicationUser application)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repo.Delete(application);
                _unitOfWork.Commit();

            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public ApplicationUser? Get(
            Expression<Func<ApplicationUser, bool>> filter = null,
            string? propertiesName = null, 
            bool tracked = true)
        {
            return _repo.Get(filter, propertiesName, tracked);
        }

        public IEnumerable<ApplicationUser>? GetAll(Expression<Func<ApplicationUser, bool>>? filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>>? orden = null, string? propertiesName = null)
        {
            return _repo.GetAll(filter, orden, propertiesName);
        }

        public void Save(ApplicationUser applicationUser)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (applicationUser.Id == string.Empty)
                {
                    _repo.Add(applicationUser);
                }
                else
                {
                    _repo.UpDate(applicationUser);
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
