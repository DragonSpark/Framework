using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose;

sealed class Decorate<T, TNext> : IRegistrationContext where T : class where TNext : class, T
{
	readonly IServiceCollection _subject;

	public Decorate(IServiceCollection subject) => _subject = subject;

	public IServiceCollection Singleton() => _subject.Decorate<T, TNext>();

	public IServiceCollection Transient() => _subject.Decorate<T, TNext>();

	public IServiceCollection Scoped() => _subject.Decorate<T, TNext>();
}