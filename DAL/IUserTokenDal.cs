using hh_clone.DAL.Models;

namespace hh_clone.DAL
{
	public interface IUserTokenDal
	{
		Task<Guid> Create(int userId);
		Task<int?> Get(Guid tokenId);
	}
}
