using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class SubjectContext : DbContext
    {
        public SubjectContext(DbContextOptions<SubjectContext> options) : base(options) { }

        public DbSet<Subject> Subject { get; set; }

    }
}
