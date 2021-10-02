using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        }

        private async Task InitializeProductsAsync()
        {
            //var ss = TestData.Sections.GroupBy(s => s.Name)
            //    .Where(s => s.Count() > 1)
            //    .Select(s => s.Key)
            //    .ToArray();

            _logger.LogInformation("Запись секций...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Запись секций выполнена успешно");

            _logger.LogInformation("Запись брэндов...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Brands.AddRange(TestData.Brands);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Запись брэндов выполнена успешно");

            _logger.LogInformation("Запись товаров...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Products.AddRange(TestData.Products);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Запись товаров выполнена успешно");
        }
    }
}
