using DragonSpark.Sources;
using System;

namespace DragonSpark.Configuration
{
	public interface IConfigurableSource<T> : ISource<T>
	{
		IScope<T> Configuration { get; }
	}

	public class ConfigurableSource<T> : DelegatedSource<T>, IConfigurableSource<T>
	{
		public ConfigurableSource( Func<T> get ) : this( new Scope<T>( get ) ) {}
		public ConfigurableSource( IScope<T> configuration ) : base( configuration.Get )
		{
			Configuration = configuration;
		}

		public IScope<T> Configuration { get; }
	}
}