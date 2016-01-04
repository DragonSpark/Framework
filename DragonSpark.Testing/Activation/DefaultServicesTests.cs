using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using Xunit;
using Xunit.Abstractions;
using ServiceLocation = DragonSpark.Activation.ServiceLocation;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Testing.Activation
{
	[AssignExecution]
	public class DefaultServicesTests : Tests
	{
		public DefaultServicesTests( ITestOutputHelper output ) : base( output )
		{}

		[Theory, AutoData]
		public void IsAvailable()
		{
			var sut = Services.Location;

			Assert.Same( sut, ServiceLocation.Instance );

			Assert.False( sut.IsAvailable );

			sut.Assign( Factory.Create<ServiceLocator>() );

			var isAvailable = sut.IsAvailable;
			Assert.True( isAvailable );

			sut.Assign( null );
			Assert.False( sut.IsAvailable );
		}
	}
}