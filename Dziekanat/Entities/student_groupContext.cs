using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class student_groupContext: DbContext
    {
        public student_groupContext(DbContextOptions<student_groupContext> options) : base(options) { }

        public DbSet<student_group> students_groups { get; set; }
    }
}
