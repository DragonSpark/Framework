using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Scopes;

sealed class Scopes : IScopes
{
	readonly IServiceScopeFactory _scopes;

	public Scopes(IServiceScopeFactory scopes) => _scopes = scopes;

	public IServiceScope Get() => _scopes.CreateScope();
}