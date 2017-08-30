using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DATA_ACCESS;
using Microsoft.EntityFrameworkCore;
using DATA_ACCESS.Repositories;
using SERVICES;
using SHOP_NETCORE.Mappings;
using BUSINESS_OBJECTS;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DATA_ACCESS.Configuration;
using Microsoft.AspNetCore.Identity;
using SERVICES.Caching;
using Microsoft.AspNetCore.Http;

namespace SHOP_NETCORE
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<ShopOnlineDbContext>(opstions => opstions
            .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add framework services.
            services.AddMvc();

            services.AddMemoryCache();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Cookies.ApplicationCookie.LoginPath = "/admin/login";
                    options.Cookies.ApplicationCookie.AccessDeniedPath = "/admin/denied";
                })
               .AddEntityFrameworkStores<ShopOnlineDbContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // Cookie settings
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/admin/login";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOut";

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            services.AddTransient<IMemoryCacheManager, MemoryCacheManager>();
            services.AddTransient<ICacheManager, RedisCacheManager>();
            services.AddTransient<IRedisConnectionWrapper, RedisConnectionWrapper>();

            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IProducerRepository, ProducerRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IReceiptNoteRepository, ReceiptNoteRepository>();
            services.AddTransient<IReceiptNoteDetailRepository, ReceiptNoteDetailRepository>();
            services.AddTransient<IUserManagerRepository, UserManagerRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IInfomationRepository, InfomationRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();

            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IProducerService, ProducerService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IReceiptNoteService, ReceiptNoteService>();
            services.AddTransient<IReceiptNoteDetailService, ReceiptNoteDetailService>();
            services.AddTransient<IUserManagerService, UserManagerService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<IInfomationService, InfomationService>();
            services.AddTransient<ICustomerService, CustomerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            new UserRoleSeed(app.ApplicationServices.GetService<RoleManager<ApplicationRole>>()).Seed();
        }
    }
}
