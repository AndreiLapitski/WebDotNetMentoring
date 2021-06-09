using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;
using NorthwindApp.Repositories;

namespace NorthwindApp
{
    public class Startup
    {
        private const string DbConnection = "Data Source=localhost;Initial Catalog=Northwind;Integrated Security=True";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            ConfigureStorage(services);
            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        private void ConfigureStorage(IServiceCollection services)
        {
            services.AddDbContext<NorthwindContext>(optionsAction => optionsAction.UseSqlServer(DbConnection), 
                ServiceLifetime.Transient,
                ServiceLifetime.Singleton);

            services.AddSingleton<IRepository<Category>, CategoryRepository>();
            services.AddSingleton<IRepository<Product>, ProductRepository>();
        }
    }
}
