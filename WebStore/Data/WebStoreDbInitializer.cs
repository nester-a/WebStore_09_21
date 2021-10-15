using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<WebStoreDbInitializer> _logger;

        public WebStoreDbInitializer(WebStoreDB db, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<WebStoreDbInitializer> logger)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("Запуск инициализации БД");
            //var db_deleted = await _db.Database.EnsureDeletedAsync();

            //var db_created = await _db.Database.EnsureCreatedAsync();

            var pending_migrations = await _db.Database.GetPendingMigrationsAsync();
            var applied_migrations = await _db.Database.GetAppliedMigrationsAsync();
            if (pending_migrations.Any())
            {
                _logger.LogInformation($"Применение миграции: {string.Join(", ", pending_migrations)}");
                await _db.Database.MigrateAsync();
            }

            try
            {
                await InitializeProductsAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка инициализации каталога товаров");
                throw;
            }
            try
            {
                await InitializeEmployeesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка инициализации таблицы сотрудников");
                throw;
            }
            try
            {
                await InitializeIdentityAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка инициализации системы Identity");
                throw;
            }
            _logger.LogInformation("Завершение инициализации БД");
        }

        private async Task InitializeProductsAsync()
        {
            var timer = Stopwatch.StartNew();
            if (_db.Sections.Any())
            {
                _logger.LogInformation("Инилциализация БД информацией о товарах не требуется");
                return;
            }

            var sections_pool = TestData.Sections.ToDictionary(section => section.Id);
            var brands_pool = TestData.Brands.ToDictionary(brand => brand.Id);

            foreach (var child_section in TestData.Sections.Where(s => s.ParentId is not null))
            {
                child_section.Parent = sections_pool[(int)child_section.ParentId!];
            }
            foreach (var product in TestData.Products)
            {
                product.Section = sections_pool[product.SectionId];
                if(product.BrandId is { } brand_id)
                {
                    product.Brand = brands_pool[brand_id];
                }
                product.Id = 0;
                product.SectionId = 0;
                product.BrandId = null;
            }

            foreach (var section in TestData.Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }

            foreach (var brand in TestData.Brands)
            {
                brand.Id = 0;
            }

            await using (await _db.Database.BeginTransactionAsync())
            {
                _logger.LogInformation("Запись секций...");
                _db.Sections.AddRange(TestData.Sections);
                _logger.LogInformation("Запись секций выполнена успешно");
                _logger.LogInformation("Запись брэндов...");
                _db.Brands.AddRange(TestData.Brands);
                _logger.LogInformation("Запись брэндов выполнена успешно");
                _logger.LogInformation("Запись товаров...");
                _db.Products.AddRange(TestData.Products);
                _logger.LogInformation("Запись товаров выполнена успешно");
                _logger.LogInformation($"Запись в БД информациии о товарах выполнена успешно за {timer.Elapsed.TotalMilliseconds} мс");
                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
        }
        private async Task InitializeEmployeesAsync()
        {
            if (_db.Employees.Any())
            {
                _logger.LogInformation("Инилциализация БД информацией о сотрудниках не требуется");
                return;
            }

            _logger.LogInformation("Запись сотрудников...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Employees.AddRange(TestData.Employees);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Запись сотрудников выполнена успешно");
        }

        private async Task InitializeIdentityAsync()
        {
            _logger.LogInformation("Инициализаия системы Identity");
            var timer = Stopwatch.StartNew();

            //if (!await _roleManager.RoleExistsAsync(Role.Administrators))
            //    await _roleManager.CreateAsync(new Role { Name = Role.Administrators });

            async Task CheckRole(string roleName)
            {
                if (await _roleManager.RoleExistsAsync(roleName))
                    _logger.LogInformation($"Роль {roleName} существует");
                else
                {
                    _logger.LogInformation($"Роль {roleName} не существует");
                    await _roleManager.CreateAsync(new Role { Name = roleName });
                    _logger.LogInformation($"Роль {roleName} успешно создана");
                }
            }

            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);

            if(await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                _logger.LogInformation($"Пользователь {User.Administrator} не существует");

                var admin = new User()
                {
                    UserName = User.Administrator,
                };

                var creation_result = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                {
                    _logger.LogInformation($"Пользователь {User.Administrator} успешно создан");
                    await _userManager.AddToRoleAsync(admin, Role.Administrators);
                    _logger.LogInformation($"Пользователю {User.Administrator} успешно добавлена роль {Role.Administrators}");
                }
                else
                {
                    var errors = creation_result.Errors.Select(err => err.Description).ToArray();
                    _logger.LogError($"Учётная запись администратора не создана! Ошибки {string.Join(",", errors)}");

                    throw new InvalidOperationException($"Невозможно создать Администратора {string.Join(",", errors)}");
                }

                _logger.LogInformation($"Данные системы Identity успешно добавлены в БД за {timer.Elapsed.TotalMilliseconds} мс");
            }
        }
    }
}
