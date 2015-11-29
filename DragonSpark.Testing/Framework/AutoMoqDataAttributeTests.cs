using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Framework
{
	public class AutoMoqDataAttributeTests
	{
		[Theory, SetupAutoData, Test]
		public void Mocked( [Frozen]Mock<IInterface> sut, IInterface item )
		{
			Assert.Equal( sut.Object, item );
		}
	}
}
