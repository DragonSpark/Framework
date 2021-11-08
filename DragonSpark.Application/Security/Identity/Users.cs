using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity;

sealed class Users<T> : Result<UsersSession<T>>, IUsers<T> where T : class
{
	public Users(UserSessions<T> result) : base(result) {}
}