using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
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
	}
}