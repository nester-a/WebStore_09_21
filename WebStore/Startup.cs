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

            services.AddTransient<WebStoreDbInitializer>();


            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            services.AddSingleton<IProductData, InMemoryProductData>();

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
