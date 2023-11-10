using hh_clone.DAL.Models;

namespace hh_clone.DAL
{
	public interface IDbSessionDal
	{
		Task<int> Create(SessionModel model);
		Task<int> Update(SessionModel model);
		Task<SessionModel?> Get(Guid sessionid);
		Task Lock(Guid sessionid);
	}
}
