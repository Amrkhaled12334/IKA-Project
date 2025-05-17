using IKEA.DAL.Common.Enums;
using IKEA.DAL.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.Data.Configurations.EmployeeConfigurations
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).HasColumnType("varchar(50)").IsRequired();

            builder.Property(E => E.Address).HasColumnType("varchar(100)");

            builder.Property(E => E.Salary).HasColumnType("decimal(8,2)");

            builder.Property(e => e.Gender).HasConversion
                (
                  (gender) => gender.ToString(),
                  (gender) => (Gender)Enum.Parse(typeof(Gender), gender)
                );

            builder.Property(e => e.EmployeeType).HasConversion
               (
                 (Type) => Type.ToString(),
                 (Type) => (EmployeeType)Enum.Parse(typeof(EmployeeType), Type)
               );

            //Development Usage
            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GetDate()");

            builder.Property(D => D.LastModifiedOn).HasComputedColumnSql("GetDate()");
        }

    }
}
