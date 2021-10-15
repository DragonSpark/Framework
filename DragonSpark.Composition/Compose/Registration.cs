using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose;

public sealed class Registration<T> : IncludingRegistration where T : class
{
	public Registration(IServiceCollection subject, IRegistrationContext context)
		: this(subject, context.Adapt()) {}

	public Registration(IServiceCollection subject, IRegistration current) : base(subject, current) {}

	public Registration(IServiceCollection subject, IRegistrations types) : base(subject, types) {}

	public Registration<T> Decorate<TNext>() where TNext : class, T
		=> new(Services, Next(new Registrations<TNext>(Services).Then(new Decorate<T, TNext>(Services))));
}