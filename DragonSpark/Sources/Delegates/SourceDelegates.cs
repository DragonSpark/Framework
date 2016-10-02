using DragonSpark.Activation;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem.Generics;
using System;

namespace DragonSpark.Sources.Delegates
{
	public sealed class SourceDelegates : DelegatesBase
	{
		public static IParameterizedSource<IActivator, IParameterizedSource<Type, Delegate>> Sources { get; } = new Cache<IActivator, SourceDelegates>( func => new SourceDelegates( func ) );
		SourceDelegates( IActivator source ) : base( source.ToSourceDelegate(), IsSourceSpecification.Default, nameof(ToDelegate) ) {}

		protected override Delegate Create( Type parameter ) => Methods.Make( ResultTypes.Default.Get( parameter ) ).Invoke<Delegate>( Locator( parameter ) );

		static Delegate ToDelegate<T>( ISource<T> source ) => source.ToDelegate();
	}
}