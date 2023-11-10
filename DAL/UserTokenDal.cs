using hh_clone.DAL.Models;

namespace hh_clone.DAL
{
	public class UserTokenDal : IUserTokenDal
	{
		public async Task<Guid> Create(int userid)
		{
			Guid tokenid = Guid.NewGuid();
			string sql = @"insert into UserToken (UserTokenID, UserId, Created)
                    values (@tokenid, @userid, NOW())";

			await DbHelper.ExecuteScalarAsync(sql, new { userid, tokenid });
			return tokenid;
		}

		public async Task<int?> Get(Guid tokenid)
		{
			string sql = @"select UserId from UserToken where UserTokenID = @tockenid";
			return await DbHelper.ExecuteScalarAsync(sql, new { tokenid });
		}
	}
}
