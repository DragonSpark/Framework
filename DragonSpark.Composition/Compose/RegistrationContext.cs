using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose
{
	public sealed class RegistrationContext<T> where T : class
	{
		readonly IServiceCollection _collection;

		public RegistrationContext(IServiceCollection collection) => _collection = collection;

		public IServiceCollection Singleton<TResult>() where TResult : class, IResult<T>
			=> _collection.AddSingleton<TResult>()
			              .AddSingleton(x => x.GetRequiredService<TResult>().ToDelegate())
			              .AddSingleton(x => x.GetRequiredService<TResult>().Get());

		public IServiceCollection FromEnvironment()
			=> _collection.AddSingleton(A.Type<T>(),
			                            _collection.GetRequiredInstance<IComponentType>().Get(A.Type<T>()));
	}
}