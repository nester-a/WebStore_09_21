using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;
        public SqlProductData(WebStoreDB db) => _db = db;
        public IEnumerable<Brand> GetBrands() => _db.Brands;
        public Brand GetBrandById(int id) => _db.Brands.SingleOrDefault(b => b.Id == id);

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section);



            if (filter?.Ids.Length > 0)
            {
                query = query.Where(product => filter.Ids.Contains(product.Id));
            }
            else
            {
                if (filter?.SectionId is { } section_id)
                {
                    query = query.Where(p => p.SectionId == section_id);
                }
                if (filter?.BrandId is { } brand_id)
                {
                    query = query.Where(p => p.BrandId == brand_id);
                }
            }

            return query;
        }
        public Product GetProductById(int id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefault(p => p.Id == id);

        public IEnumerable<Section> GetSections() => _db.Sections;
        public Section GetSectionById(int id) => _db.Sections.SingleOrDefault(s => s.Id == id);

    }
}
