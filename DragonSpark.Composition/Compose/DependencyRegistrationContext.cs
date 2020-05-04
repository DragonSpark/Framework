using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;
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

			SingletonRegistrations() : base(x => new RegisterDependencies(x, x.AddSingleton!)) {} // TODO: https://youtrack.jetbrains.com/issue/RSRP-479406
		}

		sealed class ScopedRegistrations : Store<IServiceCollection, ICommand<Type>>
		{
			public static ScopedRegistrations Default { get; } = new ScopedRegistrations();

			ScopedRegistrations() : base(x => new RegisterDependencies(x, x.AddScoped!)) {}
		}

		sealed class TransientRegistrations : Store<IServiceCollection, ICommand<Type>>
		{
			public static TransientRegistrations Default { get; } = new TransientRegistrations();

			TransientRegistrations() : base(x => new RegisterDependencies(x, x.AddTransient!)) {}
		}
	}
}