using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using Microsoft.Practices.Unity;
using Xunit;

namespace DragonSpark.Testing.Setup.Commands
{
	public class ConfigureUnityCommandTests
	{
		[Theory, Test, SetupAutoData( typeof(UnitySetup) )]
		public void Extension( IUnityContainer sut )
		{
			Assert.NotNull( sut.Configure<TestExtension>() );
		}
	}
}