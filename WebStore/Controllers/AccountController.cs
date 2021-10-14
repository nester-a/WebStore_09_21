using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Register

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                UserName = model.UserName,
            };
            var register_result = await _userManager.CreateAsync(user, model.Password);

            if (register_result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _userManager.AddToRoleAsync(user, Role.Users);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in register_result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        #endregion

        #region Login

        public IActionResult Login(string returnUrl) => View(new LoginViewModel() { ReturnUrl = returnUrl });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var loginResult = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                false);

            if (loginResult.Succeeded)
            {
                //return Redirect(model.ReturnUrl);// Не безопасно!!!
                //if(Url.IsLocalUrl(model.ReturnUrl)) return Redirect(model.ReturnUrl);
                //RedirectToAction("Index", "Home");

                return LocalRedirect(model.ReturnUrl ?? "/");
            }
            ModelState.AddModelError("", "Ошибка ввода имени пользователя, или пароля");

            return View(model);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}
