using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose
{
	sealed class ResultRegistration<T, TResult> : IRegistrationContext
		where T : class where TResult : class, IResult<T>
	{
		readonly IServiceCollection _collection;

		public ResultRegistration(IServiceCollection collection) => _collection = collection;

		public IServiceCollection Singleton()
			=> _collection.AddSingleton<TResult>()
			              .AddSingleton(x => x.GetRequiredService<TResult>().ToDelegate())
			              .AddSingleton(x => x.GetRequiredService<TResult>().Get());

		public IServiceCollection Transient()
			=> _collection.AddTransient<TResult>()
			              .AddTransient(x => x.GetRequiredService<TResult>().ToDelegate())
			              .AddTransient(x => x.GetRequiredService<TResult>().Get());

		public IServiceCollection Scoped()
			=> _collection.AddScoped<TResult>()
			              .AddScoped(x => x.GetRequiredService<TResult>().ToDelegate())
			              .AddScoped(x => x.GetRequiredService<TResult>().Get());
	}
}