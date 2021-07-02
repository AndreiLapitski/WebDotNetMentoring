using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthwindApp.Filters;
using NorthwindApp.Helpers;
using NorthwindApp.Interfaces;
using NorthwindApp.Middleware;
using NorthwindApp.Models;
using NorthwindApp.Repositories;

namespace NorthwindApp
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
            services.AddSingleton(Configuration);
            ConfigureStorage(services);
            services.AddRazorPages();

            services.AddMvc().AddMvcOptions(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.MaxModelValidationErrors = 999999; // https://stackoverflow.com/questions/63112368/asp-net-core-api-validationvisitor-exceeded-the-maximum-configured-validation
                    options.Filters.Add(typeof(LogFilter));
                });
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
                app.UseExceptionHandler(builder => builder.UseCustomErrorHandler());
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<CacheMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    "DownloadImage",
                    "images/{categoryId?}",
                    new
                    {
                        controller = "Picture",
                        action = "DownloadCategoryPicture"
                    });
            });

            ClearOrCreateCacheDirectory(
                env,
                Configuration.GetValue<string>(Constants.CachedFolderNameKey));
        }

        private void ConfigureStorage(IServiceCollection services)
        {
            services.AddDbContext<NorthwindContext>(optionsAction =>
                    optionsAction.UseSqlServer(Configuration.GetConnectionString(Constants.DbConnectionKey)));

            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Supplier>, SupplierRepository>();
        }

        private void ClearOrCreateCacheDirectory(IWebHostEnvironment env, string directoryName)
        {
            string path = $"{env.ContentRootPath}\\{directoryName}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                ClearCacheDirectory(path);
            }
        }

        private void ClearCacheDirectory(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
