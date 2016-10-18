using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Configuration
{
	public abstract class ConfigurationProvisionedParameterizedFactoryBase<TConfiguration, TResult> : ConfigurationProvisionedParameterizedFactoryBase<TConfiguration, object, TResult>, IConfigurationProvisionedFactory<TConfiguration, TResult>
	{
		protected ConfigurationProvisionedParameterizedFactoryBase( Func<object, TConfiguration> seed, Func<object, ImmutableArray<IAlteration<TConfiguration>>> configurators, Func<TConfiguration, object, TResult> factory ) : base( seed, configurators, factory ) {}
	}

	public abstract class ConfigurationProvisionedParameterizedFactoryBase<TConfiguration, TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>, IConfigurationProvisionedFactory<TConfiguration, TParameter, TResult>
	{
		readonly Func<TConfiguration, TParameter, TResult> factory;

		protected ConfigurationProvisionedParameterizedFactoryBase( Func<TParameter, TConfiguration> seed, Func<TParameter, ImmutableArray<IAlteration<TConfiguration>>> configurators, Func<TConfiguration, TParameter, TResult> factory ) : 
			this( new ParameterizedScope<TParameter, TConfiguration>( seed ), new ParameterizedScope<TParameter, ImmutableArray<IAlteration<TConfiguration>>>( configurators ), factory ) {}

		protected ConfigurationProvisionedParameterizedFactoryBase( IParameterizedScope<TParameter, TConfiguration> seed, IParameterizedScope<TParameter, ImmutableArray<IAlteration<TConfiguration>>> configurators, Func<TConfiguration, TParameter, TResult> factory )
		{
			Seed = seed;
			Configurators = configurators;
			this.factory = factory;
		}

		public IParameterizedScope<TParameter, TConfiguration> Seed { get; }

		public IParameterizedScope<TParameter, ImmutableArray<IAlteration<TConfiguration>>> Configurators { get; }

		public override TResult Get( TParameter parameter )
		{
			var configured = Configurators.Get( parameter ).Aggregate( Seed.Get( parameter ), ( configuration, transformer ) => transformer.Get( configuration ) );
			var result = factory( configured, parameter );
			return result;
		}
	}
}