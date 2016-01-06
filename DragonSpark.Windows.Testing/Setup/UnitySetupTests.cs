using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.Unity;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Windows.Testing.Setup
{
	public class UnitySetupTests : Tests
	{
		public UnitySetupTests( ITestOutputHelper output ) : base( output )
		{}

		[Theory, UnitySetup.AutoData]
		public void Extension( [Located]IUnityContainer sut )
		{
			Assert.NotNull( sut.Configure<TestExtension>() );
		}

		[Theory, UnitySetup.AutoData]
		public void RegisteredName( [Located]IUnityContainer sut )
		{
			Assert.NotNull( sut.Resolve<Singleton>( "SomeName" ) );
		}
	}
}