using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Environment;
using Microsoft.Extensions.DependencyInjection;
using System;

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
		{
			var type           = A.Type<T>();
			var implementation = _collection.GetRequiredInstance<IComponentType>().Get(type);
			var result         = _collection.AddSingleton(type, new Selector(implementation).Get);
			return result;
		}

		sealed class Selector : ISelect<IServiceProvider, object>
		{
			readonly Type _type;

			public Selector(Type type) => _type = type;

			public object Get(IServiceProvider parameter) => parameter.GetService(_type);
		}
	}
}