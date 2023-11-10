using hh_clone.BL.Auth;
using hh_clone.BL.Exceptions;
using hh_clone.MiddleWares;
using hh_clone.ViewMapper;
using hh_clone.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace hh_clone.Controllers
{
	[SiteNotAuthorizeAttribue()]
	public class RegisterController: Controller
    {
        private readonly IAuth authBl;

        public RegisterController(IAuth authBl)
        {
            this.authBl = authBl;
        }

        [HttpGet]
        [Route("/register")]
        public IActionResult Index()
        {
            return View("Index", new RegisterViewModel());
        }

        [HttpPost]
        [Route("/register")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> IndexSave(RegisterViewModel model)
        {
			if (ModelState.IsValid)
			{
                try
                {
					await authBl.Register(AuthMapper.MapRegisterViewModelToUserModel(model));
					return Redirect("/");
				}
				catch (DuplicatedEmailException)
                {
                    ModelState.TryAddModelError("Email", "Email уже сущесвтует");
                }
			}

			return View("Index", model);
        }

    }
}
