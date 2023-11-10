using hh_clone.BL.General;
using hh_clone.DAL;

namespace hh_clone.BL.Auth
{
	public class CurrentUser : ICurrentUser
	{
		private readonly IDbSession dbSession;
		private readonly IWebCookie webCookie;
		private readonly IUserTokenDal userTokenDal;

		public CurrentUser(
			IUserTokenDal userTokenDal,
			IDbSession dbSession,
			IWebCookie webCookie) 
		{
			this.userTokenDal = userTokenDal;
			this.webCookie = webCookie;
			this.dbSession = dbSession;
		}

		public async Task<int?> GetUserIdByToken()
		{
			string? tokenCookie = webCookie.Get(AuthConstants.RememberMeCookieName);
			if (tokenCookie is null)
				return null;

			Guid? tokenGuid = Helpers.StringToGuidDef(tokenCookie ?? "");
			if (tokenGuid is null)
				return null;
			int? userId = await userTokenDal.Get((Guid)tokenGuid);

			return userId;
		}

		public async Task<bool> IsLoggedIn()
		{
			bool isLoggedIn = await dbSession.IsLoggedIn();
			
			if (!isLoggedIn)
			{
				int? userId = await GetUserIdByToken();
				if (userId is not null)
				{
					await dbSession.SetUserId((int)userId);
					isLoggedIn = true;
				}
			}

			return isLoggedIn;
		}
	}
}
