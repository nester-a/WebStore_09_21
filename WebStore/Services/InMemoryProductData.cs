using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IEnumerable<Product> query = TestData.Products;

            //if (filter?.SectionId is not null)
            //{
            //    query = query.Where(p => p.SectionId == filter.SectionId);
            //}

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

        public IEnumerable<Section> GetSections() => TestData.Sections;
    }
}
