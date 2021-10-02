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

        public IEnumerable<Product> GetProducts(ProductFilter filter)
        {
            IQueryable<Product> query = _db.Products;

            if (filter?.SectionId is { } section_id)
            {
                query = query.Where(p => p.SectionId == section_id);
            }
            if (filter?.BrandId is { } brand_id)
            {
                query = query.Where(p => p.BrandId == brand_id);
            }

            return query;
        }

        public IEnumerable<Section> GetSections() => _db.Sections;
    }
}
