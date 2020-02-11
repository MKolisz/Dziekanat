using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dziekanat.Entities
{
    public class DepartmentContext:DbContext
    {
        public DepartmentContext(DbContextOptions<DepartmentContext> options) : base(options) { }

        public DbSet<Department> Department { get; set; }
    }
}
