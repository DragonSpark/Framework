using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Configuration
{
	public class ConfigurableParameterizedSource<TParameter, TResult> : DecoratedParameterizedSource<TParameter, TResult>, IConfigurableParameterizedSource<TParameter, TResult>
	{
		public ConfigurableParameterizedSource( Func<TParameter, TResult> factory ) : this( new ParameterizedScope<TParameter, TResult>( factory ) ) {}

		[UsedImplicitly]
		public ConfigurableParameterizedSource( IParameterizedScope<TParameter, TResult> configuration ) : base( configuration )
		{
			Configuration = configuration;
		}

		public IParameterizedScope<TParameter, TResult> Configuration { get; }
	}
}