using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.Unity;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing
{
	public class RegistrationTests : Tests
	{
		public RegistrationTests( ITestOutputHelper output ) : base( output )
		{}

		[Theory, Test, SetupAutoData]
		public void Singleton( [Located] IUnityContainer sut )
		{
			var once = sut.Resolve<RegisterAsSingleton>();
			var twice = sut.Resolve<RegisterAsSingleton>();
			Assert.Same( once, twice );
		}

		[Theory, Test, SetupAutoData]
		public void Many( [Located] IUnityContainer sut )
		{
			var once = sut.Resolve<RegisterAsMany>();
			var twice = sut.Resolve<RegisterAsMany>();
			Assert.NotSame( once, twice );
		}

		/*[Fact]
		public void Sandbox()
		{
			var temp = typeof(IList).GetMethod( "Add" );
			
			IList sut = new UnityInstanceCollection();

			sut.Add( new Object() );

			// temp.Invoke( sut, new [] { new UnityInstance() } );

			Debugger.Break();
		}*/
	}
}