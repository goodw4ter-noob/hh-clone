namespace hh_clone.BL.Auth
{
	public interface ICurrentUser
	{
		Task<bool> IsLoggedIn();
	}
}
