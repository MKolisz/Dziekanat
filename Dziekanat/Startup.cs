using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Dziekanat.Entities;

namespace Dziekanat
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<StudentContext>(options => options.UseSqlServer(Configuration.GetSection("Db")["ConnectionString"]));
            services.AddDbContext<DepartmentContext>(options => options.UseSqlServer(Configuration.GetSection("Db")["ConnectionString"]));
            services.AddDbContext<EmployeeContext>(options => options.UseSqlServer(Configuration.GetSection("Db")["ConnectionString"]));
            services.AddDbContext<ClassesContext>(options => options.UseSqlServer(Configuration.GetSection("Db")["ConnectionString"]));
            services.AddDbContext<GradeContext>(options => options.UseSqlServer(Configuration.GetSection("Db")["ConnectionString"]));
            services.AddDbContext<GroupContext>(options => options.UseSqlServer(Configuration.GetSection("Db")["ConnectionString"]));
            services.AddDbContext<SubjectContext>(options => options.UseSqlServer(Configuration.GetSection("Db")["ConnectionString"]));
            services.AddDbContext<student_groupContext>(options => options.UseSqlServer(Configuration.GetSection("Db")["ConnectionString"]));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
