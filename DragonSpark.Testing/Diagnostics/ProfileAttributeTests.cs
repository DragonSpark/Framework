using DragonSpark.Aspects;
using DragonSpark.Diagnostics;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using DragonSpark.Testing.Framework;
using Xunit;
using Xunit.Abstractions;
using TimedOperationFactory = DragonSpark.Testing.Framework.Diagnostics.TimedOperationFactory;

namespace DragonSpark.Testing.Diagnostics
{
	public class ProfileAttributeTests : TestCollectionBase
	{
		const string OverridingMethodTemplate = "Overriding Method Template";
		public ProfileAttributeTests( ITestOutputHelper output ) : base( output ) {}

		[Fact]
		public void Verify()
		{
			var history = LoggingHistory.Default.Get();
			Assert.Empty( history.Events );
			HelloWorld();
			var item = Assert.Single( history.Events );
			var text = item.MessageTemplate.Text;
			Assert.Contains( OverridingMethodTemplate, text );
		}

		[Fact]
		public void AssignedSource()
		{
			var configuration = TimedOperations.Configuration.Implementation;
			configuration.Assign( TimedOperationFactory.Default.Wrap() );

			var history = LoggingHistory.Default.Get();
			Assert.Empty( history.Events );
			HelloWorldConfigured();
			var item = Assert.Single( history.Events );
			var text = item.MessageTemplate.Text;
			Assert.Contains( TimedOperationFactory.ExecutedTestMethodMethod, text );

			const string template = "Template";
			var one = configuration.Get( template );
			var two = configuration.Get( template );
			Assert.Same( one, two );
		}

		[Timed( OverridingMethodTemplate )]
		static void HelloWorld() {}


		[Timed]
		static void HelloWorldConfigured() {}
	}
}
