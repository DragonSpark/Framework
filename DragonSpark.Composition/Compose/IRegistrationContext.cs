using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose;

public interface IRegistrationContext
{
	IServiceCollection Singleton();

	IServiceCollection Transient();

	IServiceCollection Scoped();
}