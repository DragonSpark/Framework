using DragonSpark.Setup;
using DragonSpark.Testing.Framework;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Setup
{
	public class SetupTests
	{
		[Theory, Test, SetupAutoData]
		public void MockAsExpected( DragonSpark.Setup.Setup sut )
		{
			Assert.NotNull( Mock.Get( sut ) );
		}

		[Theory, Test, SetupAutoData]
		public void SetupRegistered( ISetup sut )
		{
			Assert.IsType<DefaultSetup>( sut );
		}
	}
}
