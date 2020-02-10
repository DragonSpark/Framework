using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	public sealed class DependencyRegistrationContext : IRegistrationContext
	{
		readonly IServiceCollection   _collection;
		readonly IRegistrationContext _context;
		readonly Type                 _source;

		public DependencyRegistrationContext(IServiceCollection collection, IRegistrationContext context, Type source)
		{
			_collection = collection;
			_context    = context;
			_source     = source;
		}

		public IServiceCollection Singleton()
		{
			SingletonRegistrations.Default.Get(_collection).Execute(_source);
			return _context.Singleton();
		}

		public IServiceCollection Transient()
		{
			TransientRegistrations.Default.Get(_collection).Execute(_source);
			return _context.Transient();
		}

		public IServiceCollection Scoped()
		{
			ScopedRegistrations.Default.Get(_collection).Execute(_source);
			return _context.Scoped();
		}

		sealed class SingletonRegistrations : Store<IServiceCollection, ICommand<Type>>
		{
			public static SingletonRegistrations Default { get; } = new SingletonRegistrations();

			SingletonRegistrations() : base(x => new RegisterDependencies(x, x.AddSingleton)) {}
		}

		sealed class ScopedRegistrations : Store<IServiceCollection, ICommand<Type>>
		{
			public static ScopedRegistrations Default { get; } = new ScopedRegistrations();

			ScopedRegistrations() : base(x => new RegisterDependencies(x, x.AddScoped)) {}
		}

		sealed class TransientRegistrations : Store<IServiceCollection, ICommand<Type>>
		{
			public static TransientRegistrations Default { get; } = new TransientRegistrations();

			TransientRegistrations() : base(x => new RegisterDependencies(x, x.AddTransient)) {}
		}
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

	sealed class GenericTypeDependencySelector : ValidatedAlteration<Type>, IActivateUsing<Type>
	{
		public GenericTypeDependencySelector(Type type)
			: base(Start.A.Selection.Of.System.Type.By.Returning(IsGenericTypeDefinition.Default.Get(type)),
			       GenericTypeDefinition.Default.Then()
			                            .Ensure.Input.Is(IsDefinedGenericType.Default)
			                            .Otherwise.Use(x => x)) {}
	}
}