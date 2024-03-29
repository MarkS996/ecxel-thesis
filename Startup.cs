﻿using ExcelDatabaseThesis.Data;
using ExcelDatabaseThesis.Repositories;
using ExcelDatabaseThesis.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExcelDatabaseThesis
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
            // Context illetve lazy load beállítása
            services.AddDbContext<ExcelContext>(options => options.UseLazyLoadingProxies().UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ExcelService>();
            services.AddScoped<ExcelTableRepository>();
            services.AddScoped<ExcelRowRepository>();
            services.AddScoped<ExcelCellRepository>();
            services.AddScoped<FileDataRepository>();
            services.AddScoped<FilterRepository>();

            // Rest controllereknél NewtonsoftJson beállítása, request-ben null értékek engedélyezése
            services.AddControllers().AddNewtonsoftJson().AddJsonOptions(jsonOptions => jsonOptions.JsonSerializerOptions.IgnoreNullValues = true);
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
