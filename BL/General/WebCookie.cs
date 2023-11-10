using System.Security.Cryptography.X509Certificates;

namespace hh_clone.BL.General
{
	public class WebCookie : IWebCookie
	{
		public IHttpContextAccessor httpContextAccessor;

		public WebCookie(
				IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

		public void AddSecure(string cookieName, string value, int days = 0)
		{
			CookieOptions options = new CookieOptions();
			options.Path = "/";
			options.Secure = true;
			options.HttpOnly = true;

			if (days > 0)
				options.Expires = DateTimeOffset.Now.AddDays(days);
			httpContextAccessor?.HttpContext?.Response.Cookies.Append(cookieName, value, options);
		}	

		public void Add(string cookieName, string value, int days = 0)
		{
			CookieOptions options = new CookieOptions();
			options.Path = "/";
			if (days > 0)
				options.Expires = DateTimeOffset.Now.AddDays(days);
			httpContextAccessor?.HttpContext?.Response.Cookies.Append(cookieName, value, options);
		}

		public void Delete(string cookieName)
		{
			httpContextAccessor?.HttpContext?.Response.Cookies.Delete(cookieName);
		}

		public string? Get(string cookieName)
		{
			var cookie = httpContextAccessor?.HttpContext?.Request?.Cookies.FirstOrDefault((c) => c.Key == cookieName);
			if (cookie is not null && cookie.Value.Value is not null)
				return cookie.Value.Value;
			return null;
		}
	}
}
