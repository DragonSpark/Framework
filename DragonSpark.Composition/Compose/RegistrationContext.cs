using DragonSpark.Compose;
using DragonSpark.Compose.Model;
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

	public sealed class RegistrationContext<TFrom, TTo> : IRegistrationContext
		where TTo : class, TFrom where TFrom : class
	{
		readonly IServiceCollection _collection;

		public RegistrationContext(IServiceCollection collection) => _collection = collection;

		public IServiceCollection Singleton() => _collection.AddSingleton<TFrom, TTo>();

		public IServiceCollection Transient() => _collection.AddTransient<TFrom, TTo>();

		public IServiceCollection Scoped() => _collection.AddScoped<TFrom, TTo>();
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

	sealed class ConfigureFromEnvironment : SelectedCommand<IServiceCollection>
	{
		public static ConfigureFromEnvironment Default { get; } = new ConfigureFromEnvironment();

		ConfigureFromEnvironment() : base(ServiceConfigurationLocator.Default.Get) {}
	}

	sealed class ServiceConfigurationLocator : LocateComponent<IServiceCollection, IServiceConfiguration>
	{
		public static ServiceConfigurationLocator Default { get; } = new ServiceConfigurationLocator();

		ServiceConfigurationLocator() : base(x => x.GetRequiredInstance<IComponentType>()) {}
	}

	public class LocateComponent<TIn, TOut> : Select<TIn, TOut>
	{
		public LocateComponent(Func<TIn, IComponentType> select) : this(select.Start()) {}

		public LocateComponent(Selector<TIn, IComponentType> select)
			: base(select.Select(x => x.Get(A.Type<TOut>()))
			             .Select(Start.A.Selection.Of.System.Type.By.Self.Get()
			                          .Then()
			                          .Activate<TOut>()
			                          .Ensure.Input.IsAssigned.Otherwise.Throw(LocateGuardMessage.Default)
			                          .Ensure.Output.IsAssigned.Otherwise.Throw(LocateComponentMessage<TOut>.Default)
			                    )) {}
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