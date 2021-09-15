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
        /// ������ �������� ���������� ��� ����, ����� �� �����
        /// �������� ������ � ������������ � ��������� ������ ������
        /// ����������.
        /// ������������ ������������ � .��� ��� �������� ��, ��� �� ����� � 
        /// ������ ����� � �������� ������ ����������
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// ����������� ��� �������� ������������
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //����� ���������� ������������ � ��������������
            //�� ������������� ������ � ��� � ������� ���������
            services.AddControllersWithViews();
        }

        /// <summary>
        /// ����� ����������� ��������, ������� ������������ �������� �����������
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //����� ������������ ��������� ��������� ����������
                //���� �� ��������� � ������ ������������
                app.UseDeveloperExceptionPage();
            }

            // ����� ������������ �������������
            app.UseRouting();


            var greetings = "Hello from my first ASP.NET Core APP";

            //��������� ��������� ����� ������������ �� ����� ��������
            //��������� ������ �������� Configuration
            var configurationGreetings = Configuration["Greetings"];
            var logging = Configuration["Logging:LogLevel:Default"];

            //����� ���������� �������� ��������
            app.UseEndpoints(endpoints =>
            {
                //��������� ������� �� �������� "/"
                endpoints.MapGet("/greetings", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });

                //�������� ����� - ��� ������, � ������� ����� ����������� � ������� ��������
                //��� ������ ����������
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                //���� ����� ��� �
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
