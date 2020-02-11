using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class GradeContext : DbContext
    {
        public GradeContext(DbContextOptions<GradeContext> options)
            : base(options)
        {
        }

        public DbSet<Grade> Grade { get; set; }

    }
}
