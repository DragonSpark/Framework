using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Configuration
{
	public abstract class ConfigurableParameterizedFactoryBase<TConfiguration, TResult> : ConfigurableParameterizedFactoryBase<TConfiguration, object, TResult>, IConfigurableFactory<TConfiguration, TResult>
	{
		protected ConfigurableParameterizedFactoryBase( Func<object, TConfiguration> seed, Func<TConfiguration, object, TResult> factory ) : this( seed, o => Items<IAlteration<TConfiguration>>.Immutable, factory ) {}
		protected ConfigurableParameterizedFactoryBase( Func<object, TConfiguration> seed, Func<object, ImmutableArray<IAlteration<TConfiguration>>> configurators, Func<TConfiguration, object, TResult> factory ) : base( seed, configurators, factory ) {}
		// protected ConfigurableParameterizedFactoryBase( IParameterizedScope<TConfiguration> seed, IParameterizedScope<ImmutableArray<IAlteration<TConfiguration>>> configurators, Func<TConfiguration, object, TResult> factory ) : base( seed, configurators, factory ) {}
	}

	public interface IConfigurableFactory<TConfiguration, out TResult> : IConfigurableFactory<TConfiguration, object, TResult> {}

	public interface IConfigurableFactory<TConfiguration, TParameter, out TResult> : IParameterizedSource<TParameter, TResult>
	{
		IParameterizedScope<TParameter, TConfiguration> Seed { get; }
		
		IParameterizedScope<TParameter, ImmutableArray<IAlteration<TConfiguration>>> Configurators { get; }
	}

	public abstract class ConfigurableParameterizedFactoryBase<TConfiguration, TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>, IConfigurableFactory<TConfiguration, TParameter, TResult>
	{
		readonly Func<TConfiguration, TParameter, TResult> factory;

		protected ConfigurableParameterizedFactoryBase( Func<TParameter, TConfiguration> seed, Func<TParameter, ImmutableArray<IAlteration<TConfiguration>>> configurators, Func<TConfiguration, TParameter, TResult> factory ) : 
			this( new ParameterizedScope<TParameter, TConfiguration>( seed ), new ParameterizedScope<TParameter, ImmutableArray<IAlteration<TConfiguration>>>( configurators ), factory ) {}

		protected ConfigurableParameterizedFactoryBase( IParameterizedScope<TParameter, TConfiguration> seed, IParameterizedScope<TParameter, ImmutableArray<IAlteration<TConfiguration>>> configurators, Func<TConfiguration, TParameter, TResult> factory )
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