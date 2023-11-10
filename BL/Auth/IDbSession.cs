using hh_clone.DAL.Models;

namespace hh_clone.BL.Auth
{
	public interface IDbSession
	{
		Task<SessionModel> GetSession();

		Task<int> SetUserId(int userId);

		Task<int?> GetUserId();

		Task<bool> IsLoggedIn();

		Task Lock();

		void ResetSessionCache();
	}
}
