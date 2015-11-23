using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Testing.Framework
{
	public static class FixtureExtensions
	{
		public static IServiceLocator GetLocator( this IFixture @this )
		{
			var result = Get<ServiceLocatorCustomization, IServiceLocator>( @this, customization => customization.Locator );
			return result;
		}

		public static IRecordingLogger GetLogger( this IFixture @this )
		{
			var result = Get<OutputCustomization, IRecordingLogger>( @this, customization => customization.Logger );
			return result;
		}

		public static MethodInfo GetCurrentMethod( this IFixture @this )
		{
			var result = Get<CurrentMethodCustomization, MethodInfo>( @this, customization => customization.Method );
			return result;
		}

		static TResult Get<TCustomization, TResult>( IFixture fixture, Func<TCustomization, TResult> resolve )
		{
			var result = fixture.GetItems().FirstOrDefaultOfType<TCustomization>().Transform( resolve );
			return result;
		}

		public static IList<ICustomization> GetItems( this IFixture @this )
		{
			var result = AmbientValues.Get<IList<ICustomization>>( @this );
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