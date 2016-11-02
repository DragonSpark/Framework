using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Sources.Scopes
{
	public abstract class ScopedParameterizedSource<TParameter, TResult> : DecoratedParameterizedSource<TParameter, TResult>
	{
		protected ScopedParameterizedSource() : this( parameter => default(TResult) ) {}
		protected ScopedParameterizedSource( Func<object, Func<TParameter, TResult>> global ) : this( new ParameterizedScope<TParameter, TResult>( global ) ) {}
		protected ScopedParameterizedSource( Func<TParameter, TResult> factory ) : this( new ParameterizedScope<TParameter, TResult>( factory ) ) {}

		[UsedImplicitly]
		protected ScopedParameterizedSource( IParameterizedScope<TParameter, TResult> configuration ) : base( configuration ) {}
	}

	public class ConfigurableParameterizedSource<TParameter, TResult> : ScopedParameterizedSource<TParameter, TResult>, IConfigurableParameterizedSource<TParameter, TResult>
	{
		public ConfigurableParameterizedSource() : this( parameter => default(TResult) ) {}
		public ConfigurableParameterizedSource( Func<object, Func<TParameter, TResult>> global ) : this( new ParameterizedScope<TParameter, TResult>( global ) ) {}
		public ConfigurableParameterizedSource( Func<TParameter, TResult> factory ) : this( new ParameterizedScope<TParameter, TResult>( factory ) ) {}

		[UsedImplicitly]
		public ConfigurableParameterizedSource( IParameterizedScope<TParameter, TResult> configuration ) : base( configuration )
		{
			Configuration = configuration;
		}

		public IParameterizedScope<TParameter, TResult> Configuration { get; }
	}
}