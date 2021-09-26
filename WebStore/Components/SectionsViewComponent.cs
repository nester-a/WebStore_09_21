using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Components
{
    public class SectionsViewComponent :ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
