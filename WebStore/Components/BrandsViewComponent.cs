using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();

        //public async Task<IViewComponentResult> InvokeAsync() => View();
    }
}
