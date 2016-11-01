using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Activation
{
	public static class ActivatorExtensions
	{
		public static Func<Type, T> Delegate<T>( this Func<IActivator> @this ) => Delegates<T>.Default.Get( @this );
		sealed class Delegates<T> : Cache<Func<IActivator>, Func<Type, T>>
		{
			public static Delegates<T> Default { get; } = new Delegates<T>();
			Delegates() : base( source => new Factory( source ).Apply( new DeferredSpecification<Type>( source ) ).ToDelegate() ) {}

			sealed class Factory : ParameterizedSourceBase<Type, T>
			{
				readonly Func<IServiceProvider> source;
				public Factory( Func<IServiceProvider> source )
				{
					this.source = source;
				}

				public override T Get( Type parameter ) => source().Get<T>( parameter );
			}
		}

		public static T Construct<T>( this IConstructor @this, params object[] parameters ) => Construct<T>( @this, typeof(T), parameters );

		public static T Construct<T>( this IConstructor @this, Type type, params object[] parameters ) => (T)@this.Get( new ConstructTypeRequest( type, parameters ) );

		public static ImmutableArray<T> ActivateMany<T>( this IActivator @this, IEnumerable<Type> types ) => @this.ActivateMany<T>( typeof(T), types );
		public static ImmutableArray<T> ActivateMany<T>( this IActivator @this, Type objectType, IEnumerable<Type> types ) => @this.CreateMany( types.Where( objectType.Adapt().IsAssignableFrom ) ).OfType<T>().ToImmutableArray();
	}
}