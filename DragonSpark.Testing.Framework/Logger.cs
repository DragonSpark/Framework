using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.Runtime;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Testing.Framework
{
	public static class FixtureExtensions
	{
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
	}

	public interface ILoggerPlayback
	{
		void Playback( Action<string> write );
	}

	class Logger : ILogger, ILoggerPlayback
	{
		readonly IList<Tuple<DateTimeOffset, string>> items = new List<Tuple<DateTimeOffset, string>>();

		public void Information( string message, Priority priority )
		{
			Write( nameof(Information), message, priority );
		}

		public void Warning( string message, Priority priority )
		{
			Write( nameof(Warning), message, priority );
		}

		public void Exception( string message, Exception item )
		{
			Write( nameof(Exception), message, Priority.High );
		}

		public void Fatal( string message, Exception exception )
		{
			Write( nameof(Fatal), message, Priority.Highest );
		}

		void Write( string type, string message, Priority priority )
		{
			var line = string.Format( CultureInfo.InvariantCulture, Resources.DefaultTextLoggerPattern, DateTimeOffset.Now, type, message, priority );
			items.Add( new Tuple<DateTimeOffset, string>( DateTimeOffset.Now, line ) );
		}

		public void Playback( Action<string> write )
		{
			items.OrderBy( x => x.Item1 ).Apply( tuple => write( tuple.Item2 ) );
		}
	}

	[Priority( Priority.AboveHigher )]
	public class EnableOutputAttribute : BeforeAfterTestAttribute, ICustomization
	{
		public void Customize( IFixture fixture )
		{
			var logger = new Logger();

			fixture.Inject<ILogger>( new CompositeLogger( new[] { logger, fixture.TryCreate<ILogger>() }.NotNull().ToArray() ) );

			fixture.Inject<ILoggerPlayback>( logger );
		}

		public override void After( MethodInfo methodUnderTest )
		{
			base.After( methodUnderTest );

			var output = AmbientValues.Get<ITestOutputHelper>( methodUnderTest.DeclaringType );
			var playback = AmbientValues.Get<IServiceLocation>( methodUnderTest ).Locator.GetInstance<ILoggerPlayback>();
			
			playback.Playback( output.WriteLine );
		}
	}
}