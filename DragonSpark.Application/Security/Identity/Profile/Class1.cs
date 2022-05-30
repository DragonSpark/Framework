using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Profile;

internal class Class1 {}

public interface IUserReference<T> : IAltering<T> where T : class {}

sealed class UserReference<T> : EvaluateToSingle<T, T>, IUserReference<T> where T : class
{
	public UserReference(IStandardScopes scopes) : base(scopes, SelectUserReference<T>.Default) {}
}

sealed class SelectUserReference<T> : StartWhere<T, T> where T : class {
	public static SelectUserReference<T> Default { get; } = new();

	SelectUserReference() : base((parameter, x) => parameter == x) {}
}