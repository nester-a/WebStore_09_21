using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Components
{
    public class UserInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => User.Identity?.IsAuthenticated == true
            ? View("UserInfo")
            : View();
    }
}
