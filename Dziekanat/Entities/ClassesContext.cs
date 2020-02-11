using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class ClassesContext : DbContext
    {
        public ClassesContext(DbContextOptions<ClassesContext> options)
            : base(options)
        {
        }

        public DbSet<Classes> Classes { get; set; }

    }
}
