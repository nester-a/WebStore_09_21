using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();
        Section GetSectionById(int id);
        IEnumerable<Brand> GetBrands();
        Brand GetBrandById(int id);
        IEnumerable<Product> GetProducts(ProductFilter filter = null);
        Product GetProductById(int id);
    }
}
