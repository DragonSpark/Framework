using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Environment;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	public sealed class RegistrationContext<TFrom, TTo> : IRegistrationContext
		where TTo : class, TFrom where TFrom : class
	{
		readonly IServiceCollection _collection;

		public RegistrationContext(IServiceCollection collection) => _collection = collection;

		public IRegistrationContext WithDependencies
			=> new DependencyRegistrationContext(_collection, this, A.Type<TTo>());

		public IServiceCollection Singleton() => _collection.AddSingleton<TFrom, TTo>();

		public IServiceCollection Transient() => _collection.AddTransient<TFrom, TTo>();

		public IServiceCollection Scoped() => _collection.AddScoped<TFrom, TTo>();
	}

	public sealed class RegistrationContext : IRegistrationContext
	{
		readonly IServiceCollection _collection;
		readonly Type               _type;

		public RegistrationContext(IServiceCollection collection, Type type)
		{
			_collection = collection;
			_type       = type;
		}

		public IRegistrationContext WithDependencies => new DependencyRegistrationContext(_collection, this, _type);

		public IServiceCollection Singleton() => _collection.AddSingleton(_type);

		public IServiceCollection Transient() => _collection.AddTransient(_type);

		public IServiceCollection Scoped() => _collection.AddScoped(_type);
	}

	public sealed class RegistrationContext<T> where T : class
	{
		readonly IServiceCollection                     _collection;
		readonly IGeneric<ISelect<IServiceProvider, T>> _generic;

		public RegistrationContext(IServiceCollection collection) : this(collection, Selector.Default) {}

		public RegistrationContext(IServiceCollection collection, IGeneric<ISelect<IServiceProvider, T>> generic)
		{
			_collection = collection;
			_generic    = generic;
		}

		public ResultRegistrationContext<T, TResult> Use<TResult>() where TResult : class, IResult<T>
			=> new ResultRegistrationContext<T, TResult>(_collection);

		public RegistrationContext<T, TTo> Map<TTo>() where TTo : class, T
			=> new RegistrationContext<T, TTo>(_collection);

		public RegistrationContext As => new RegistrationContext(_collection, A.Type<T>());

		public SelectionRegistrationContext<T> UseEnvironment()
		{
			var parameter      = A.Type<T>();
			var implementation = _collection.GetRequiredInstance<IComponentType>().Get(parameter);
			var selector       = _generic.Get(parameter, implementation);
			var result         = new SelectionRegistrationContext<T>(_collection, selector.Get);
			return result;
		}

		sealed class Selector : Generic<ISelect<IServiceProvider, T>>
		{
			public static Selector Default { get; } = new Selector();

			Selector() : base(typeof(Selector<>)) {}
		}

		sealed class Selector<TTo> : Select<IServiceProvider, T> where TTo : class, T
		{
			[UsedImplicitly]
			public static Selector<TTo> Default { get; } = new Selector<TTo>();

			Selector() : base(x => x.GetService<TTo>()) {}
		}
	}
}