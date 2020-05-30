using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose
{
	sealed class Forward<T, TTo> : IRegistrationContext where T : class where TTo : class, T
	{
		readonly IServiceCollection _subject;

		public Forward(IServiceCollection subject) => _subject = subject;

		public IServiceCollection Singleton() => _subject.AddSingleton<T, TTo>();

		public IServiceCollection Transient() => _subject.AddTransient<T, TTo>();

		public IServiceCollection Scoped() => _subject.AddScoped<T, TTo>();
	}
}