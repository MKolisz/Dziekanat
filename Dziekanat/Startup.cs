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
using AutoMapper;
using Dziekanat.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Dziekanat.Services;

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

            services.AddCors();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                    //    var employeeService = context.HttpContext.RequestServices.GetRequiredService<IEmployeeService>();
                    //    var employeeId = int.Parse(context.Principal.Identity.Name);
                    //    var employee = employeeService.GetById(employeeId);
                    //    if (employee == null)
                    //    {
                    //        // return unauthorized if user no longer exists
                    //        context.Fail("Unauthorized");
                    //    }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IGradeService, GradeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}
