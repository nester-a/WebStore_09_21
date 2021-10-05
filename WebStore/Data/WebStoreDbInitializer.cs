using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _logger;

        public WebStoreDbInitializer(WebStoreDB db, ILogger<WebStoreDbInitializer> logger)
        {
            _db = db;
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

            await InitializeProductsAsync();
            await InitializeEmployeesAsync();
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
    }
}
