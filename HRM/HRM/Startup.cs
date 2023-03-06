using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using HrmCore.Services;
using HrmInfrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCROSPolicy",
                    builder =>
                    {
                        builder.WithOrigins("*")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
          
            // TIÊM: XỬ LÝ DI: for generic interface (khong can quan tam)
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity Job
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IJobRepository, JobRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity Department
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity BasicSalary
            services.AddScoped<IBasicSalaryService, BasicSalaryService>();
            services.AddScoped<IBasicSalaryRepository, BasicSalaryRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity coefficient
            services.AddScoped<ICoefficientssalaryService, CoefficientssalaryService>();
            services.AddScoped<ICoefficientssalaryRepository, CoefficientssalaryRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity LaborContract
            services.AddScoped<ILaborContractService, LaborContractService>();
            services.AddScoped<ILaborContractRepository, LaborContractRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity Staff
            services.AddScoped<IStaffService,StaffService>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity Terminationcontract
            services.AddScoped<ITerminationcontractService, TerminationcontractService>();
            services.AddScoped<ITerminationcontractRepository, TerminationcontractRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity Position
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity Tax
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<ITaxRepository, TaxRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity TimeKeeping
            services.AddScoped<ITimeKeepingService, TimeKeepingService>();
            services.AddScoped<ITimeKeepingRepository, TimeKeepingRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity DetailSalary
            services.AddScoped<IDetailSalaryService, DetailSalaryService>();
            services.AddScoped<IDetailSalaryRepository, DetailSalaryRepository>();
            // TIÊM: XỬ LÝ DI: for service and repository interface of entity Insurance
            services.AddScoped<IInsuranceService, InsuranceService>();
            services.AddScoped<IInsuranceRepository, InsuranceRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HRM", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRM v1"));
            }
           
            app.UseHttpsRedirection();
           
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
