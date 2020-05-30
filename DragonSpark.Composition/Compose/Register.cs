using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose
{
	sealed class Register<T> : IRegistrationContext where T : class
	{
		readonly IServiceCollection _subject;

		public Register(IServiceCollection subject) => _subject = subject;

		public IServiceCollection Singleton() => _subject.AddSingleton<T>();

		public IServiceCollection Transient() => _subject.AddTransient<T>();

		public IServiceCollection Scoped() => _subject.AddScoped<T>();
	}
}