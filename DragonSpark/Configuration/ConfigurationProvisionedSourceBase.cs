using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Linq;

namespace DragonSpark.Configuration
{
	public abstract class ConfigurationProvisionedSourceBase<T> : ConfigurationProvisionedSourceBase<T, T>
	{
		protected ConfigurationProvisionedSourceBase( Func<T> seed ) : this( seed, Items<IAlteration<T>>.Default ) {}
		protected ConfigurationProvisionedSourceBase( Func<T> seed, params IAlteration<T>[] configurations ) : this( seed, new ConfigurationScope<T>( configurations ), Delegates<T>.Self ) {}
		protected ConfigurationProvisionedSourceBase( Func<T> seed, IConfigurationScope<T> scope, Func<T, T> factory ) : base( seed, scope, factory ) {}
		/*protected ConfigurableFactoryBase( IScope<T> seed ) : this( seed, new ConfigurationScope<T>( Items<IAlteration<T>>.Default ), Delegates<T>.Self ) {}
		protected ConfigurableFactoryBase( IScope<T> seed, IConfigurationScope<T> scope, Func<T, T> factory ) : base( seed, scope, factory ) {}*/
	}

	public abstract class ConfigurationProvisionedSourceBase<TConfiguration, TResult> : SourceBase<TResult>
	{
		readonly Func<TConfiguration, TResult> factory;
		
		protected ConfigurationProvisionedSourceBase( Func<TConfiguration> seed, IConfigurationScope<TConfiguration> scope, Func<TConfiguration, TResult> factory ) : 
			this( new Scope<TConfiguration>( seed ), scope, factory ) {}

		protected ConfigurationProvisionedSourceBase( IScope<TConfiguration> seed, IConfigurationScope<TConfiguration> scope, Func<TConfiguration, TResult> factory )
		{
			Seed = seed;
			Configurators = scope;
			this.factory = factory;
		}

		public IScope<TConfiguration> Seed { get; }

		public IConfigurationScope<TConfiguration> Configurators { get; }

		public override TResult Get()
		{
			var seed = Seed.Get();
			var configurations = Configurators.Get();
			var configured = configurations.Aggregate( seed, ( current, transformer ) => transformer.Get( current ) );
			var result = factory( configured );
			return result;
		}
	}
}