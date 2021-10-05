using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure.Convaentions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services.Interfaces;
using WebStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;
using WebStore.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;

namespace WebStore
{
    public class Startup
    {
        /// <summary>
        /// Данное свойство необходимо для того, чтобы мы могли
        /// получить доступ к конфигурации в различных частях нашего
        /// приложения.
        /// Особенностью конфигурации в .НЕТ Кор является то, что мы можем её 
        /// менять прямо в процессе работы приложения
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Конструктор для внесения конфигурации
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreDB>(opt => 
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServer")));

            services.AddIdentity<User, Role>(/*opt=> { opt. }*/)
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt=> 
            {

#if DEBUG
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            });

            services.ConfigureApplicationCookie(opt=> 
            {
                opt.Cookie.Name = "GD.WebStore";
                opt.Cookie.HttpOnly = true;

                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            services.AddTransient<WebStoreDbInitializer>();

            services.AddScoped<IEmployeesData, SqlEmployeesData>();
            services.AddScoped<IProductData, SqlProductData>();

            //после добавления контроллеров с представлением
            //мы конфигурируем доступ к ним с помощью маршрутов
            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention())).AddRazorRuntimeCompilation();
        }

        /// <summary>
        /// Здесь формируется конвейер, который обрабатывает входящие подключения
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //здесь подключается страничка обработки исключений
                //если мы находимся в режиме разработчика
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStatusCodePagesWithRedirects("~/home/status/{0}");

            //подключение статических ресурсов
            app.UseStaticFiles();

            // здесь подключается маршрутизация
            app.UseRouting();

            app.UseMiddleware<TestMiddleware>();
            app.UseWelcomePage("/welcome");

            //здесь начинается обработа запросов
            app.UseEndpoints(endpoints =>
            {
                //обработка запроса по маршруту "/"
                endpoints.MapGet("/greetings", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });

                //конечные точки - это адреса, к которым можно подключится с помощью браузера
                //или других приложений
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                //тоже самое что и
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
