using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Extensions;
using DragonSpark.Testing.Framework.Setup.Location;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class OutputCustomization : CustomizationBase, ITestExecutionAware
	{
		[Activate]
		public MessageLocator Locator { get; set; }

		protected override void Customize( IFixture fixture )
		{
			fixture.Items().Add( this );
		}

		void ITestExecutionAware.Before( IFixture fixture, MethodInfo methodUnderTest )
		{}

		public void After( IFixture fixture, MethodInfo methodUnderTest )
		{
			new OutputValue( methodUnderTest.DeclaringType ).Item.With( output =>
			{
				var messages = Locator.Create().OrderBy( line => line.Time ).Select( line => line.Text ).ToArray();
				messages.Each( output.WriteLine );
			} );
		}
	}

	public class MessageLocator : Diagnostics.MessageLocator
	{
		readonly IFixture fixture;

		public MessageLocator( IFixture fixture, IUnityContainer container, RecordingMessageLogger logger ) : base( container, logger )
		{
			this.fixture = fixture;
		}

		protected override IEnumerable<RecordingMessageLogger> DetermineLoggers()
		{
			var result = fixture.TryCreate<RecordingMessageLogger>().Append( base.DetermineLoggers() );
			return result;
		}
	}
}