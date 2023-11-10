using hh_clone.BL.Auth;
using hh_clone.BL.Exceptions;
using hh_clone.MiddleWares;
using hh_clone.ViewMapper;
using hh_clone.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hh_clone.Controllers
{
    [SiteNotAuthorizeAttribue()]
    public class LoginController : Controller
    {

        private readonly IAuth authBl;

        public LoginController(IAuth authBl)
        {
            this.authBl = authBl;
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult Index()
        {
            return View("Index", new LoginViewModel());
        }

        [HttpPost]
        [Route("/login")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> IndexSave(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
					await authBl.Authenticate(model.Email!, model.Password!, model.RememberMe == true);
					return Redirect("/");
				}
                catch (AuthorizationException) 
                {
                    ModelState.AddModelError("Email", "Имя или Email неверные");
                }
            }

            return View("Index", model);
        }

    }
}
