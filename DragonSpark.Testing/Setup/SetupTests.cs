using DragonSpark.Setup;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using Xunit;
using Xunit.Abstractions;
using DefaultSetup = DragonSpark.Testing.Framework.Setup.DefaultSetup;

namespace DragonSpark.Testing.Setup
{
	public class SetupTests : Tests
	{
		public SetupTests( ITestOutputHelper output ) : base( output )
		{}

		/*[Theory, Test, SetupAutoData]
		public void MockAsExpected( ISetup sut )
		{
			Assert.NotNull( Mock.Get( sut ) );
		}*/

		[Theory, Test, SetupAutoData]
		public void SetupRegistered( ISetup sut )
		{
			Assert.IsType<DefaultSetup>( sut );
		}
	}
}
