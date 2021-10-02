using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.DAL.Context
{
    class WebStoreDB : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Brand> Brands { get; set; }


        //контекст базы данных
        public WebStoreDB(DbContextOptions<WebStoreDB> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder model)
        //{
        //    base.OnModelCreating(model);

        //}
    }
}
