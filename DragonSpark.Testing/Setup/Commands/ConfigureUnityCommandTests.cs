using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.Unity;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Setup.Commands
{
	public class ConfigureUnityCommandTests : Tests
	{
		public ConfigureUnityCommandTests( ITestOutputHelper output ) : base( output )
		{}

		[Theory, Test, SetupAutoData( typeof(UnitySetup) )]
		public void Extension( [Located]IUnityContainer sut )
		{
			Assert.NotNull( sut.Configure<TestExtension>() );
		}

		[Theory, Test, SetupAutoData( typeof( UnitySetup ) )]
		public void RegisteredName( [Located]IUnityContainer sut )
		{
			Assert.NotNull( sut.Resolve<Singleton>( "SomeName" ) );
		}

		/*[Theory, Test, SetupAutoData( typeof( UnitySetup ) )]
		public void EnsureCatalog( [Located]IModuleCatalog sut )
		{
			Assert.IsType<AssemblyModuleCatalog>( sut );
		}*/
	}
}