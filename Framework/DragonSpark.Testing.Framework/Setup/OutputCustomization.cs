using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Extensions;
using Ploeh.AutoFixture;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Framework.Setup
{
	public class OutputCustomization : ICustomization, IAfterTestAware
	{
		readonly IList<Func<IRecordingLogger>> loggers = new List<Func<IRecordingLogger>>();

		/*public OutputCustomization() : this( new RecordingLogger() )
		{}

		public OutputCustomization( IRecordingLogger logger )
		{
			Logger = logger;
		}*/

		[Activate]
		public IRecordingLogger Logger { get; set; }

		public void Customize( IFixture fixture )
		{
			fixture.Items().Add( this );
			Logger.Information( "Logger initialized!" );
			Register( () => Logger );
			Register( fixture.TryCreate<IRecordingLogger> );
		}

		public void Register( Func<IRecordingLogger> resolve )
		{
			loggers.Add( resolve );
		}

		public void After( IFixture fixture, MethodInfo methodUnderTest )
		{
			AmbientValues.Get<ITestOutputHelper>( methodUnderTest.DeclaringType ).With( output =>
			{
				var lines = loggers.Select( func => func() ).NotNull().Distinct().SelectMany( aware => aware.Lines ).OrderBy( line => line.Time ).Select( line => line.Message ).ToArray();
				lines.Each( output.WriteLine );
			} );
		}
	}
}