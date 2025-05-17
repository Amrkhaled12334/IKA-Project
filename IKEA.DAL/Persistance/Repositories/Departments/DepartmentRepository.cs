using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Data;
using IKEA.DAL.Persistance.Repositories._Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.Repositories.Departments
{
    public class DepartmentRepository :GenericRepository<Department>,IDepartmentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public DepartmentRepository(ApplicationDbContext context):base(context)//ASK For Generate Object of Context
        {
            dbContext = context;
        }
    }
}
