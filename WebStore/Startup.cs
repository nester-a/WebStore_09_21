using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            //после добавления контроллеров с представлением
            //мы конфигурируем доступ к ним с помощью маршрутов
            services.AddControllersWithViews();
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
            }

            // здесь подключается маршрутизация
            app.UseRouting();


            var greetings = "Hello from my first ASP.NET Core APP";

            //результат изменения файла конфигурации мы можем получить
            //благодаря нашему свойству Configuration
            var configurationGreetings = Configuration["Greetings"];
            var logging = Configuration["Logging:LogLevel:Default"];

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
