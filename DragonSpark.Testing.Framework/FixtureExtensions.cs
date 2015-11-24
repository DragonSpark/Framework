using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;

namespace DragonSpark.Testing.Framework
{
	public static class FixtureExtensions
	{
		static TResult Get<T, TResult>( IFixture fixture, Func<T, TResult> resolve )
		{
			var result = fixture.Items().FirstOrDefaultOfType<T>().Transform( resolve );
			return result;
		}

		public static IList<ICustomization> Items( this IFixture @this )
		{
			var result = AmbientValues.Get<IList<ICustomization>>( @this );
			return result;
		}

		public static T Item<T>( this IFixture @this )
		{
			var result = Get<T, T>( @this, item => item );
			return result;
		}

		public static void InjectWithImplementation<T>( this IFixture @this, T instance )
		{
			@this.Inject( instance );
			var type = instance.GetType();
			if ( typeof(T) != type )
			{
				typeof(FixtureRegistrar).InvokeGenericAction( "Inject", new []{ type }, @this, instance );
			}
		}

		public static T TryCreate<T>( this IFixture @this ) where T : class
		{
			try
			{
				return @this.Create<T>();
			}
			catch ( ObjectCreationException )
			{
				return null;
			}
		}

		public static T TryCreate<T>( this IFixture @this, Type type )
		{
			try
			{
				var result = (T)new SpecimenContext( @this ).Resolve( type );
				return result;
			}
			catch ( ObjectCreationException )
			{
				return default(T);
			}
		}
	}
}