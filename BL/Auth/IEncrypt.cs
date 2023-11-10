namespace hh_clone.BL.Auth
{
	public interface IEncrypt
	{
		string HashPassword(string password, string salt);
	}
}
