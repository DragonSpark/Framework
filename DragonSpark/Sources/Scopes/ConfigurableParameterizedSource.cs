using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Sources.Scopes
{
	public abstract class ScopedParameterizedSourceBase<TParameter, TResult> : DecoratedParameterizedSource<TParameter, TResult>
	{
		protected ScopedParameterizedSourceBase() : this( parameter => default(TResult) ) {}
		protected ScopedParameterizedSourceBase( Func<object, Func<TParameter, TResult>> global ) : this( new ParameterizedScope<TParameter, TResult>( global ) ) {}
		protected ScopedParameterizedSourceBase( Func<TParameter, TResult> factory ) : this( new ParameterizedScope<TParameter, TResult>( factory ) ) {}

		[UsedImplicitly]
		protected ScopedParameterizedSourceBase( IParameterizedScope<TParameter, TResult> configuration ) : base( configuration ) {}
	}

	public abstract class ScopedParameterizedSourceWithImplementedFactoryBase<TParameter, TResult> : ScopedParameterizedSourceBase<TParameter, TResult>
	{
		protected ScopedParameterizedSourceWithImplementedFactoryBase() : this( parameter => default(TResult) ) {}
		protected ScopedParameterizedSourceWithImplementedFactoryBase( Func<object, Func<TParameter, TResult>> global ) : this( new ParameterizedScope<TParameter, TResult>( global ) ) {}
		protected ScopedParameterizedSourceWithImplementedFactoryBase( Func<TParameter, TResult> factory ) : this( new ParameterizedScope<TParameter, TResult>( factory ) ) {}

		[UsedImplicitly]
		protected ScopedParameterizedSourceWithImplementedFactoryBase( IParameterizedScope<TParameter, TResult> configuration ) : base( configuration )
		{
			configuration.Assign( new Func<TParameter, TResult>( Create ).GlobalCache() );
		}

		protected abstract TResult Create( TParameter parameter );
	}

	public class ConfigurableParameterizedSource<TParameter, TResult> : ScopedParameterizedSourceBase<TParameter, TResult>, IConfigurableParameterizedSource<TParameter, TResult>
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

	public abstract class ConfigurableParameterizedSourceWithImplementedFactoryBase<TParameter, TResult> : ConfigurableParameterizedSource<TParameter, TResult>
	{
		protected ConfigurableParameterizedSourceWithImplementedFactoryBase() : this( new ParameterizedScope<TParameter, TResult>( parameter => default(TResult) ) ) {}
		
		[UsedImplicitly]
		protected ConfigurableParameterizedSourceWithImplementedFactoryBase( IParameterizedScope<TParameter, TResult> configuration ) : base( configuration )
		{
			configuration.Assign( new Func<TParameter, TResult>( Create ).GlobalCache() );
		}

		protected abstract TResult Create( TParameter parameter );
	}
}