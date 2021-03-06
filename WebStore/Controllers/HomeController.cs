using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Действие данного контроллера будет доступно, если мы перейдёи по ссылке
        ///  http://localhost:5000/Home/Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View();
        public IActionResult Error404() => View();
        public IActionResult Cart() => View();
        public IActionResult CheckOut() => View();
        public IActionResult ContactUs() => View();
        public IActionResult Login() => View();
        public IActionResult ProductDetail() => View();
        public IActionResult Shop() => View();

        public IActionResult Status(string id) => id switch
        {
            "404" => View("Error404"),
            _ => Content($"Status code - {id}"),
        };
    }
}