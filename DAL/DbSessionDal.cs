﻿using hh_clone.DAL.Models;
using Dapper;
using Npgsql;

namespace hh_clone.DAL
{
	public class DbSessionDal : IDbSessionDal
	{

		public async Task<int> Create(SessionModel model)
		{
			string sql = @"insert into DbSession (DbSessionID, SessionData, Created, LastAccessed, UserId)
                    values (@DbSessionID, @SessionContent, @Created, @LastAccessed, @UserId)";

			return await DbHelper.ExecuteScalarAsync(sql, model);
		}

		public async Task<SessionModel?> Get(Guid sessionId)
		{
			string sql = @"select DbSessionID, SessionData, Created, LastAccessed, UserId from DbSession where DbSessionID = @sessionId";
			var sessions = await DbHelper.QueryAsync<SessionModel>(sql, new { sessionId });
			return sessions.FirstOrDefault();
		}

		public async Task Lock(Guid sessionId)
		{
			string sql = @"select DbSessionID from DbSession where DbSessionID = @sessionId for update";
			await DbHelper.QueryAsync<SessionModel>(sql, new { sessionId });
		}

		public async Task<int> Update(SessionModel model)
		{
			string sql = @"update DbSession
                    set SessionData = @SessionData, LastAccessed = @LastAccessed, UserId = @UserId
                    where DbSessionID = @DbSessionID
            ";

			return await DbHelper.ExecuteScalarAsync(sql, model);
		}
	}
}
