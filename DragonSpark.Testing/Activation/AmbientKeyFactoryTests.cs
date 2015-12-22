using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Testing.Activation
{
	public class AmbientKeyFactoryTests
	{
		[Theory, AutoData]
		public void Assign( AmbientKeyFactory sut, [Modest]ServiceLocator locator, AmbientValueRepository repository )
		{
			var key = sut.Create( locator );
			repository.Add( key, locator );

			new[] { locator, null }.Each( serviceLocator =>
			{
				Assert.Equal( locator, repository.Get( new AmbientRequest( typeof(IServiceLocator), serviceLocator ) ) );
			} );
		}
	}
}