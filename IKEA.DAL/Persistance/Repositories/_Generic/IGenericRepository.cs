using IKEA.DAL.Models;
using IKEA.DAL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.Repositories._Generic
{
    public interface IGenericRepository<T> where T :ModelBase
    {
        IQueryable<T> GetAll(bool WithNoTracking = true);

        Task<T>? GetById(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
