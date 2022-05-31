using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class UserReference<T> : EvaluateToSingle<T, T>, IUserReference<T> where T : class
{
	public UserReference(IStandardScopes scopes) : base(scopes, SelectUserReference<T>.Default) {}
}