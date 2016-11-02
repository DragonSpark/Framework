using System;

namespace DragonSpark.Sources.Scopes
{
	public interface IConfigurableSource<T> : ISource<T>
	{
		IScope<T> Configuration { get; }
	}

	public class ConfigurableSource<T> : ScopedSource<T>, IConfigurableSource<T>
	{
		public ConfigurableSource() : this( () => default(T) ) {}
		public ConfigurableSource( Func<T> factory ) : this( factory.Create() ) {}
		public ConfigurableSource( IScope<T> scope ) : base( scope )
		{
			Configuration = scope;
		}

		public IScope<T> Configuration { get; }
	}
}