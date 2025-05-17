using IKEA.DAL.Models;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.Repositories._Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext context)//ASK For Generate Object of Context
        {
            dbContext = context;
        }
        public IQueryable<T> GetAll(bool WithNoTracking = true)
        {// In-Memory Collection
            // IQuerable : DataBase Collection
            if (WithNoTracking)
                return dbContext.Set<T>().AsNoTracking();

            return dbContext.Set<T>();
        }
        public async Task<T?> GetById(int id)
        {
            var item = await dbContext.Set<T>().FindAsync(id);

            //var Department = dbContext.Departments.Local.FirstOrDefault(D=>D.Id == id);
        
            //if(Department is null)
            //    Department = dbContext.Departments.FirstOrDefault(D=>D.Id == id);

            return item;
        }
        public void Add(T item)
        {
            dbContext.Set<T>().Add(item);
        }
        public void Update(T item)
        {
            dbContext.Set<T>().Update(item);
        }
        public void Delete(T item)
        {
            item.IsDeleted = true;
            dbContext.Set<T>().Update(item);
        }
    }
}
