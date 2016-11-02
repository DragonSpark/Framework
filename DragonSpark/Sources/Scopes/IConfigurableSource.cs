using System;

namespace DragonSpark.Sources.Scopes
{
	public interface IConfigurableSource<T> : ISource<T>
	{
		IScope<T> Configuration { get; }
	}

	public class ConfigurableSource<T> : ScopedSourceBase<T>, IConfigurableSource<T>
	{
		public ConfigurableSource( Func<T> factory ) : this( factory.Create() ) {}
		public ConfigurableSource( IScope<T> scope ) : base( scope )
		{
			Configuration = scope;
		}

		public IScope<T> Configuration { get; }
	}

	public abstract class ConfigurableSourceWithImplementedFactoryBase<T> : ConfigurableSource<T>
	{
		protected ConfigurableSourceWithImplementedFactoryBase() : this( new Scope<T>() ) {}
		protected ConfigurableSourceWithImplementedFactoryBase( IScope<T> scope ) : base( scope )
		{
			scope.Assign( new Func<T>( Create ).GlobalCache() );
		}

		protected abstract T Create();
	}
}