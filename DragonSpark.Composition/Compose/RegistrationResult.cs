using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose;

public readonly struct RegistrationResult
{
	public RegistrationResult(IServiceCollection then) => Then = then;

	public IServiceCollection Then { get; }
}