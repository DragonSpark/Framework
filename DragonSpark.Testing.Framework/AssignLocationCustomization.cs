using System.Reflection;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public class AssignLocationCustomization : ICustomization, IAfterTestAware
	{
		readonly IFactory<MethodInfo, IAmbientKey> factory;
		readonly IServiceLocation location;

		public AssignLocationCustomization() : this( ServiceLocation.Instance )
		{}

		public AssignLocationCustomization( IServiceLocation location ) : this( location, AmbientLocatorKeyFactory.Instance )
		{}

		public AssignLocationCustomization( IServiceLocation location, IFactory<MethodInfo, IAmbientKey> factory )
		{
			this.factory = factory;
			this.location = location;
		}

		public void Customize( IFixture fixture )
		{
			var locator = fixture.GetLocator();
			var key = factory.Create( fixture.GetCurrentMethod() );
			locator.Register( key );
			location.Assign( locator );

			locator.Register( location );
			
		}

		public void After( IFixture fixture, MethodInfo methodUnderTest )
		{
			location.Assign( null );
		}
	}
}