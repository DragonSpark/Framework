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
		readonly ICollection<Func<IMessageRecorder>> register = new List<Func<IMessageRecorder>>();

		readonly ICollection<IMessageRecorder> loggers = new Collection<IMessageRecorder>();

		[Activate]
		public IMessageRecorder MessageRecorder { get; set; }

		public void Customize( IFixture fixture )
		{
			fixture.Items().Add( this );
			MessageRecorder.Information( "Logger initialized!" );
			Register( () => MessageRecorder );
			Register( fixture.TryCreate<IMessageRecorder> );
		}

		public void Register( Func<IMessageRecorder> resolve )
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
				var lines = loggers.SelectMany( aware => aware.Messages ).OrderBy( line => line.Time ).Select( line => line.Text ).ToArray();
				lines.Each( output.WriteLine );
			} );
		}
	}
}