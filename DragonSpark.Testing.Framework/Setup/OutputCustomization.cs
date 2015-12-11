using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Extensions;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Framework.Setup
{
	public class OutputCustomization : ICustomization, ITestExecutionAware
	{
		readonly ICollection<Func<IRecordingLogger>> register = new List<Func<IRecordingLogger>>();

		readonly ICollection<IRecordingLogger> loggers = new Collection<IRecordingLogger>();

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
			register.Add( resolve );
		}

		public void Before( IFixture fixture, MethodInfo methodUnderTest )
		{
			loggers.Clear();
			loggers.AddRange( register.Select( func => func() ).NotNull().Distinct() );
		}

		public void After( IFixture fixture, MethodInfo methodUnderTest )
		{
			AmbientValues.Get<ITestOutputHelper>( methodUnderTest.DeclaringType ).With( output =>
			{
				var lines = loggers.SelectMany( aware => aware.Lines ).OrderBy( line => line.Time ).Select( line => line.Message ).ToArray();
				lines.Each( output.WriteLine );
			} );
		}
	}
}