using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Scopes;

sealed class Scoping : IScoping
{
	readonly IServiceScopeFactory _scopes;

	public Scoping(IServiceScopeFactory scopes) => _scopes = scopes;

	public AsyncServiceScope Get() => _scopes.CreateAsyncScope();
}