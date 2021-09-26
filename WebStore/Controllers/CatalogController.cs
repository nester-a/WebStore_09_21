using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;

        public CatalogController(IProductData productData)
        {
            _productData = productData;
        }
        public IActionResult Index(int? brandId, int? sectionId)
        {
            var filter = new ProductFilter()
            {
                BrandId = brandId,
                SectionId = sectionId,
            };

            var products = _productData.GetProducts(filter);

            var view_model = new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.OrderBy(p => p.Order)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                })
            };

            return View(view_model);
        }
    }
}
