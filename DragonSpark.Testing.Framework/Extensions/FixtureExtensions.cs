using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup.Location;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Setup;

namespace DragonSpark.Testing.Framework.Extensions
{
	public static class FixtureExtensions
	{
		public static IServiceLocator GetLocator( this IFixture @this )
		{
			var result = Get<ServiceLocationCustomization, IServiceLocator>( @this, customization => customization.Locator );
			return result;
		}

		public static MethodInfo GetMethod( this IFixture @this )
		{
			var result = Get<CurrentMethodCustomization, MethodInfo>( @this, customization => customization.Method );
			return result;
		}

		static TResult Get<T, TResult>( IFixture fixture, Func<T, TResult> resolve ) where T : ICustomization
		{
			var result = fixture.Items().With( list => list.FirstOrDefaultOfType<T>().With( resolve ) );
			return result;
		}

		public static IList<ICustomization> Items( this IFixture @this )
		{
			var result = new Items<ICustomization>( @this ).Item;
			return result;
		}

		public static T Item<T>( this IFixture @this ) where T : ICustomization
		{
			var result = Get<T, T>( @this, item => item );
			return result;
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