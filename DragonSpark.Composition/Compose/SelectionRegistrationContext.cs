using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose {
	public sealed class SelectionRegistrationContext<T> : IRegistrationContext where T : class
	{
		readonly IServiceCollection        _collection;
		readonly Func<IServiceProvider, T> _select;

		public SelectionRegistrationContext(IServiceCollection collection, Func<IServiceProvider, T> select)
		{
			_collection = collection;
			_select     = select;
		}

		public IServiceCollection Singleton() => _collection.AddSingleton(_select);

		public IServiceCollection Transient() => _collection.AddTransient(_select);

		public IServiceCollection Scoped() => _collection.AddScoped(_select);
	}
}