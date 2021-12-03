using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using GarbageDBTest.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using GarbageDBTest.Domain.Interfaces;
using GarbageDBTest.Infraestructure.Repositories;
using Microsoft.AspNetCore.Http;
using GarbageDBTest.Application.Services;
using AutoMapper;
using FluentValidation;
using GarbageDBTest.Infraestructure.Validators;
using GarbageDBTest.Domain.Dtos.Request;

namespace GarbageDBTest
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GarbageDBTest", Version = "v1" });
            });
            services.AddDbContext<GarbageDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GarbageDB")));
            
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddTransient<IEventRepository, EventSqlRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPOIRepository, PoiSqlRepository>();
            services.AddScoped<IApiServices,ApiServices>();
                                          //Que se va a validar y quien lo va a validar
            services.AddScoped<IValidator<POICreateRequest>, POICreateRequestValidator>();
            services.AddScoped<IValidator<EventCreateRequest>, EventCreateRequestValidator>();
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GarbageDBTest v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
