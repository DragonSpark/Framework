using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using Xunit;
using Xunit.Abstractions;
using ServiceLocation = DragonSpark.Activation.ServiceLocation;
using ServiceLocatorFactory = DragonSpark.Testing.Objects.Setup.ServiceLocatorFactory;

namespace DragonSpark.Testing.Activation
{
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

			var serviceLocator = ServiceLocatorFactory.Instance.Create();
			Assert.NotNull( serviceLocator );
			sut.Assign( serviceLocator );

			var isAvailable = sut.IsAvailable;
			Assert.True( isAvailable );

			sut.Assign( null );
			Assert.False( sut.IsAvailable );
		}
	}
}