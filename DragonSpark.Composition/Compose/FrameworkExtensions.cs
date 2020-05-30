using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose
{
	static class FrameworkExtensions
	{
		public static IRegistration Then(this IRegistration @this, IRegistration next)
			=> new LinkedRegistrationContext(@this, next);

		public static IRegistrations Then(this IRegistrations @this, IRegistrationContext context)
			=> @this.Then(context.Adapt());

		public static IRegistrations Then(this IRegistrations @this, IRegistration context)
			=> @this.Then(context.Fixed());

		public static IRegistrations Then(this IRegistrations @this, IRegistrations next)
			=> new LinkedRegistrations(@this, next);

		public static IRegistrations Fixed(this IRegistration @this) => new FixedRegistrations(@this);

		public static IRegistration Adapt(this IRegistrationContext @this) => new Adapter(@this);

		public static RegistrationResult Result(this IServiceCollection @this) => new RegistrationResult(@this);
	}
}