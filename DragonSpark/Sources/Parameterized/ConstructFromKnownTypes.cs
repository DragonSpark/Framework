using DragonSpark.TypeSystem;
using System;
using System.Linq;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class ConstructFromKnownTypes<T> : ParameterConstructedCompositeFactory<object>, IParameterizedSource<object, T>
	{
		public static ISource<IParameterizedSource<object, T>> Default { get; } = new Scope<ConstructFromKnownTypes<T>>( Factory.GlobalCache( () => new ConstructFromKnownTypes<T>( KnownTypes.Default.Get<T>().ToArray() ) ) );
		ConstructFromKnownTypes( params Type[] types ) : base( types ) {}

		T IParameterizedSource<object, T>.Get( object parameter ) => (T)Get( parameter );
	}
}