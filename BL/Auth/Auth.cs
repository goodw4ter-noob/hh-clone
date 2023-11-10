using hh_clone.DAL.Models;
using hh_clone.DAL;
using System.ComponentModel.DataAnnotations;
using hh_clone.BL.Exceptions;
using hh_clone.BL.General;

namespace hh_clone.BL.Auth
{
    public class Auth : IAuth
    {
        private readonly IAuthDAL authDal;
        private readonly IEncrypt encrypt;
        private readonly IDbSession dbSession;
        private readonly IUserTokenDal userTokenDal;
        private readonly IWebCookie webCookie;

		public Auth(
			IWebCookie webCookie,
			IAuthDAL authDal,
            IEncrypt encrypt,
			IDbSession dbSession,
			IUserTokenDal userTokenDal)
        {
            this.webCookie = webCookie;
            this.userTokenDal = userTokenDal;
            this.authDal = authDal;
            this.encrypt = encrypt;
            this.dbSession = dbSession;
		}

        public async Task Login(int id)
        {
            await dbSession.SetUserId(id);
        }

		public async Task<int?> Authenticate(string email, string password, bool rememberMe)
        {
            var user = await authDal.GetUser(email);

            if (user.Password == encrypt.HashPassword(password, user.Salt))
            {
                await Login(user.UserId ?? 0);

                if (rememberMe) 
                {
                    Guid tokenId = await userTokenDal.Create(user.UserId ?? 0);
                    webCookie.AddSecure(AuthConstants.RememberMeCookieName, tokenId.ToString(), 30);
				}

                return user.UserId ?? 0;
            }
			throw new AuthorizationException();
		}

		public async Task<int> CreateUser(UserModel user)
		{
			user.Salt = Guid.NewGuid().ToString();
			user.Password = encrypt.HashPassword(user.Password, user.Salt);
			int id = await authDal.CreateUser(user);
			await Login(id);
			return id;
		}

		public async Task ValidateEmail(string email)
        {
			var user = await authDal.GetUser(email);

            if (user.UserId != null)
                throw new DuplicatedEmailException();
		}

		public async Task Register(UserModel user)
		{
            using (var scope = hh_clone.BL.General.Helpers.CreateTransactionScope())
            {
                await dbSession.Lock();
				await ValidateEmail(user.Email);
				await CreateUser(user);
                scope.Complete();
			}
                
		}
	}
}
