using DragonSpark.Activation;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem.Generics;
using System;

namespace DragonSpark.Sources.Delegates
{
	public sealed class ParameterizedSourceDelegates : DelegatesBase
	{
		public static IParameterizedSource<IActivator, IParameterizedSource<Type, Delegate>> Sources { get; } = new Cache<IActivator, ParameterizedSourceDelegates>( func => new ParameterizedSourceDelegates( func ) );
		ParameterizedSourceDelegates( IActivator source ) : base( source.ToSourceDelegate(), IsParameterizedSourceSpecification.Default, nameof(ToDelegate) ) {}

		protected override Delegate Create( Type parameter ) => 
			Methods
				.Make( ParameterTypes.Default.Get( parameter ), ResultTypes.Default.Get( parameter ) )
				.Invoke<Delegate>( Locator( parameter ) );

		static Delegate ToDelegate<TParameter, TResult>( IParameterizedSource<TParameter, TResult> source ) => source.ToSourceDelegate();
	}
}