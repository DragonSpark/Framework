using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

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

	public static class FixtureContext
	{
		public static IDisposable Create( IFixture item )
		{
			var local = new TaskLocalValue( item );
			var key = new AmbientKey<IFixture>( new ProvidedSpecification( () => local.Item != null ) );
			var result = new AmbientContext( key, item );
			return result;
		}

		public static IFixture GetCurrent()
		{
			var result = AmbientValues.Get<IFixture>();
			return result;
		}
	}

	public class OutputCustomization : ICustomization, IAfterTestAware
	{
		public OutputCustomization() : this( new RecordingLogger() )
		{}

		public OutputCustomization( IRecordingLogger logger )
		{
			Logger = logger;
		}

		public IRecordingLogger Logger { get; }

		public void Customize( IFixture fixture )
		{
			fixture.GetItems().Add( this );
			Logger.Information( "Logger initialized!" );
		}

		public void After( IFixture fixture, MethodInfo methodUnderTest )
		{
			AmbientValues.Get<ITestOutputHelper>( methodUnderTest.DeclaringType ).With( output =>
			{
				var lines = new[]
				{
					Logger,
					fixture.TryCreate<IRecordingLogger>(),
					fixture.GetLocator().Transform( location => location.GetInstance<IRecordingLogger>() )
				}.NotNull().Distinct().SelectMany( aware => aware.Lines ).OrderBy( line => line.Time ).Select( line => line.Message ).ToArray();

				lines.Apply( output.WriteLine );
			} );
		}
	}

	public interface IAfterTestAware
	{
		void After( IFixture fixture, MethodInfo methodUnderTest );
	}

	public class TestAttribute : BeforeAfterTestAttribute
	{
		public override void After( MethodInfo methodUnderTest )
		{
			base.After( methodUnderTest );

			AmbientValues.Get<IFixture>( methodUnderTest ).With( fixture =>
			{
				fixture.GetItems().OfType<IAfterTestAware>().Apply( aware => aware.After( fixture, methodUnderTest ) );
				AmbientValues.Remove( fixture );
			} );

			AmbientValues.Remove( methodUnderTest );
		}
	}
}