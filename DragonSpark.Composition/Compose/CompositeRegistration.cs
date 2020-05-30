using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose
{
	public sealed class CompositeRegistration : IncludingRegistration
	{
		public CompositeRegistration(IServiceCollection services, IRegistrations current) : base(services, current) {}

		public CompositeRegistration And<TNext>() where TNext : class
			=> new CompositeRegistration(Services,
			                             Next(new Registrations<TNext>(Services).Then(new Register<TNext>(Services))));
	}
}