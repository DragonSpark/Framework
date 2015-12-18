using DragonSpark.Activation.FactoryModel;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using Xunit;

namespace DragonSpark.Testing.Activation.FactoryModel
{
	public class ObjectFactoryParameterTests
	{
		[Theory, Test, SetupAutoData]
		public void Casting( ObjectFactoryParameter sut )
		{
			Assert.NotNull( sut );
		}
	}
}