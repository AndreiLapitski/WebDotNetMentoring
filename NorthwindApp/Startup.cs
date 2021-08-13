using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.OpenApi.Models;
using NorthwindApp.Filters;
using NorthwindApp.Helpers;
using NorthwindApp.Interfaces;
using NorthwindApp.Middleware;
using NorthwindApp.Models;
using NorthwindApp.Repositories;
using Constants = NorthwindApp.Helpers.Constants;

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
            services.AddAutoMapper(typeof(Startup));
            ConfigureStorage(services);
            //services.AddRazorPages();//duplicated at 54

            //AZURE AD
            //services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
            //    .AddOpenIdConnect(options => Configuration.Bind("AzureAd", options));

            //services.Configure<CookieAuthenticationOptions>(
            //    AzureADDefaults.CookieScheme, 
            //    options => options.AccessDeniedPath = "/Account/AccessDenied");

            //works
            services
                .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            
            //var authScheme = AzureADDefaults.AuthenticationScheme;

            //services
            //    .AddAuthentication(o =>
            //    {
            //        o.DefaultAuthenticateScheme = authScheme;
            //    })
            //    .AddCookie(authScheme, authScheme, opt =>
            //    {
            //        opt.Cookie.Name = "MyAuth";
            //        opt.LoginPath = "/Account/SignIn";
            //        opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            //        opt.AccessDeniedPath = "/Error/UnAuthorized";
            //        opt.LogoutPath = "/Account/SignOut";
            //    });


            //services
            //    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            //services
            //    .AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            //        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            //    })
            //    .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            //services
            //    .AddAuthentication(AzureADDefaults.AuthenticationScheme)
            //    .AddAzureAD(options => Configuration.Bind("AzureAd", options));


            services.AddControllersWithViews(options =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            //https://docs.microsoft.com/en-us/aspnet/core/security/authorization/razor-pages-authorization?view=aspnetcore-5.0
            services
                .AddRazorPages(options =>
                {
                    options.Conventions.AllowAnonymousToPage("/Index");
                })
                .AddMicrosoftIdentityUI();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                //options.LoginPath = "/Account/Login";
                //options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddMvc().AddMvcOptions(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.MaxModelValidationErrors = 999999; // https://stackoverflow.com/questions/63112368/asp-net-core-api-validationvisitor-exceeded-the-maximum-configured-validation
                    options.Filters.Add(typeof(LogFilter));
                });

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Northwind API",
                    Version = "v1"
                });
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

            app.UseAuthentication();
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API V1");
            });

            ClearOrCreateCacheDirectory(
                env,
                Configuration.GetValue<string>(Constants.CachedFolderNameKey));
        }

        private void ConfigureStorage(IServiceCollection services)
        {
            services
                .AddDbContext<NorthwindIdentityContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString(Constants.NorthwindIdentityConnectionKey)));

            services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<NorthwindIdentityContext>();

            services.AddDbContext<NorthwindContext>(optionsAction =>
                    optionsAction.UseSqlServer(Configuration.GetConnectionString(Constants.DbConnectionKey)));

            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Supplier>, SupplierRepository>();
            services.AddScoped<IEmailSender, EmailHelper>();
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
