using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public BrandsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke() => View(GetBrands());

        //public async Task<IViewComponentResult> InvokeAsync() => View();

        private IEnumerable<BrandViewModel> GetBrands() =>
            _productData.GetBrands()
            .OrderBy(b => b.Order)
            .Select(b => new BrandViewModel()
            {
                Id = b.Id,
                Name = b.Name,
            });
    }
}
