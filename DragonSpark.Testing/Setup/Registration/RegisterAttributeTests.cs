using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using Microsoft.Practices.ServiceLocation;
using Xunit;

namespace DragonSpark.Testing.Setup.Registration
{
	public class RegisterAttributeTests
	{
		[Theory, Test, SetupAutoData]
		public void RegisterWithName( IServiceLocator locator )
		{
			Assert.Null( locator.GetInstance<IRegisteredWithName>() );

			var located = locator.GetInstance<IRegisteredWithName>( "Registered" );
			Assert.IsType<RegisteredWithNameClass>( located );
		}
	}

	public interface IRegisteredWithName
	{ }


	[DragonSpark.Setup.Registration.Register( "Registered" )]
	public class RegisteredWithNameClass : IRegisteredWithName
	{ }
}