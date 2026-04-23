using Microsoft.EntityFrameworkCore;
using ShoppingMVC.Datos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Datos.Repositorios
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {

        private readonly ShoppingMvcDbContext _db;

        internal DbSet<T> dbSet { get; set; }   //internal= accesible desde todo el proyecto
                                                // DbSet representa una tabla en la bd

        public RepositorioGenerico(ShoppingMvcDbContext db)
        {
            _db=db ?? throw new ArgumentNullException("dependencias no establecidas"); //almacena el contexto de la bd
            dbSet = _db.Set<T>();  //almacena los datos de la tabla
                
        }




        public void Add(T entity)
        {
            try                              // intenta
            {
                dbSet.Add(entity);
            }
            catch (Exception)                // atrapa
            {

                throw new ArgumentException("Error al agregar una entidad");     //lanza
            }
        }

        public void Delete(T entity)
        {
            try
            {
                dbSet.Remove(entity);
            }
            catch (Exception)
            {

                throw new ArgumentException("Error al intentar borrar una entidad");
            }
        }

        public T? Get(Expression<Func<T, bool>>? filter = null, string? propertiesname = null, bool traked = true)
        {
            IQueryable<T> query = dbSet.AsQueryable();  // como consultable

            if (!string.IsNullOrWhiteSpace(propertiesname))
            {
                foreach (var property in propertiesname.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }

            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return traked ? query.FirstOrDefault() : query.AsNoTracking().FirstOrDefault();

        } //operador ternario => CONDICION ? VALOR SI TRUE : VALOR SI FALSE 

        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
            string? propertiesname = null)
        {
            IQueryable<T> query = dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(propertiesname))
            {
                foreach (var property in propertiesname.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter!= null)
            {
                query = query.Where(filter);
            }
            return query.ToList();          // retorna una lista ASNOTRACING  solo lectura
        }
    }
}
