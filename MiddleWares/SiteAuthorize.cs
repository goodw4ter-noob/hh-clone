using hh_clone.BL.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hh_clone.MiddleWares
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class SiteAuthorize : Attribute, IAsyncAuthorizationFilter
	{
		public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
			ICurrentUser? currentUser = context.HttpContext.RequestServices.GetService<ICurrentUser>();
			if (currentUser == null)
			{
				throw new Exception("No user middleware");
			}

			bool isLoggedIn = await currentUser.IsLoggedIn();
			if (isLoggedIn == false)
			{
				context.Result = new RedirectResult("/login");
				return;
			}
		}
	}
}
