using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace hh_clone.BL.Auth
{
	public class Encrypt: IEncrypt
	{
		public string HashPassword(string password, string salt)
		{
			byte[] hash = KeyDerivation.Pbkdf2(
				password,
				System.Text.Encoding.ASCII.GetBytes(salt),
				KeyDerivationPrf.HMACSHA512,
				5000,
				64
			);

			return Convert.ToBase64String( hash );
		}
	}
}
