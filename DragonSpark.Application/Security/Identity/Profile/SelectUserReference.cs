using DragonSpark.Application.Entities.Queries.Composition;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class SelectUserReference<T> : StartWhere<T, T> where T : class {
	public static SelectUserReference<T> Default { get; } = new();

	SelectUserReference() : base((parameter, x) => parameter == x) {}
}