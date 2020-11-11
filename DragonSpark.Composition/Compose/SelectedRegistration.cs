using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	sealed class SelectedRegistration<T> : IRegistrationContext where T : class
	{
		readonly IServiceCollection        _collection;
		readonly Func<IServiceProvider, T> _select;

		public SelectedRegistration(IServiceCollection collection)
			: this(collection, Selector.Default.Get(collection.Component<T>()).Start().Then().Assume()) {}

		public SelectedRegistration(IServiceCollection collection, Func<IServiceProvider, T> select)
		{
			_collection = collection;
			_select     = select;
		}

		public IServiceCollection Singleton() => _collection.AddSingleton(_select);

		public IServiceCollection Transient() => _collection.AddTransient(_select);

		public IServiceCollection Scoped() => _collection.AddScoped(_select);


		sealed class Selector : Generic<ISelect<IServiceProvider, T>>
		{
			public static Selector Default { get; } = new Selector();

			Selector() : base(typeof(Selector<>)) {}
		}

		sealed class Selector<TTo> : Select<IServiceProvider, T> where TTo : class, T
		{
			[UsedImplicitly]
			public static Selector<TTo> Default { get; } = new Selector<TTo>();

			Selector() : base(x => x.GetService<TTo>()!) {}
		}
	}
}