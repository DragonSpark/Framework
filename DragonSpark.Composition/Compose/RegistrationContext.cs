using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Environment;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	public interface IRegistrationContext
	{
		IServiceCollection Singleton();

		IServiceCollection Transient();

		IServiceCollection Scoped();
	}

	public sealed class ResultRegistrationContext<T, TResult> : IRegistrationContext
		where T : class where TResult : class, IResult<T>
	{
		readonly IServiceCollection _collection;

		public ResultRegistrationContext(IServiceCollection collection) => _collection = collection;

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

	sealed class ConfigureFromEnvironment : ICommand<IServiceCollection>
	{
		public static ConfigureFromEnvironment Default { get; } = new ConfigureFromEnvironment();

		ConfigureFromEnvironment() : this(A.Type<IServiceConfiguration>(),
		                                  Start.A.Selection.Of.System.Type.By.Self.Then()
		                                       .Activate<IServiceConfiguration>()
		                                       .Get) {}

		readonly Type                              _type;
		readonly Func<Type, IServiceConfiguration> _select;

		public ConfigureFromEnvironment(Type type, Func<Type, IServiceConfiguration> select)
		{
			_type   = type;
			_select = @select;
		}

		public void Execute(IServiceCollection parameter)
		{
			var implementation = parameter.GetRequiredInstance<IComponentType>().Get(_type);

			_select(implementation).Execute(parameter);
		}
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