using hh_clone.BL.Exceptions;
using hh_clone.BL.General;
using hh_clone.DAL;
using hh_clone.DAL.Models;

namespace hh_clone.BL.Auth
{
	public class DbSession : IDbSession
	{
		private SessionModel? sessionModel = null;
		private readonly IDbSessionDal sessionDal;
		private readonly IWebCookie webCookie;

		public DbSession(IDbSessionDal sessionDal, IWebCookie webCookie)
		{
			this.sessionDal = sessionDal;
			this.webCookie = webCookie;
		}

		public void ResetSessionCache()
		{
			sessionModel = null;
		}

		private void CreateSessionCookie(Guid sessionId)
		{
			this.webCookie.Delete(AuthConstants.SessionCookieName);
			this.webCookie.AddSecure(AuthConstants.SessionCookieName, sessionId.ToString());
		}

		private async Task<SessionModel> CreateSession()
		{
			var data = new SessionModel()
			{
				DbSessionId = Guid.NewGuid(),
				Created = DateTime.Now,
				LastAccessed = DateTime.Now
			};

			await sessionDal.Create(data);

			return data;
		}

		public async Task<SessionModel> GetSession()
		{
			if (sessionModel != null)
				return sessionModel;

			Guid sessionId;
			var sessionString = webCookie.Get(AuthConstants.SessionCookieName);
			if (sessionString != null)
				sessionId = Guid.Parse(sessionString);
			else
				sessionId = Guid.NewGuid();

			var data = await this.sessionDal.Get(sessionId);
			if (data == null)
			{
				data = await this.CreateSession();
				CreateSessionCookie(data.DbSessionId);
			}
			sessionModel = data;
			return data;
		}

		public async Task<int?> GetUserId()
		{
			var data = await this.GetSession();

			return data?.UserId;
		}

		public async Task<int> SetUserId(int userId)
		{
			var data = await this.GetSession();

			data.UserId = userId;
			data.DbSessionId = Guid.NewGuid();
			CreateSessionCookie(data.DbSessionId);
			
			return await sessionDal.Create(data);
		}

		public async Task<bool> IsLoggedIn()
		{
			var data = await this.GetSession();
			return data?.UserId != null;
		}

		public async Task Lock()
		{
			var data = await this.GetSession();
			await sessionDal.Lock(data.DbSessionId);
		}
	}
}
