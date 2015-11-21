using DragonSpark.Testing.TestObjects;
using Moq;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Framework
{
	public class AutoMoqDataAttributeTests
	{
		[Theory, Framework.AutoData( typeof(AutoConfiguredMoqCustomization) )]
		public void Mocked( [Frozen]Mock<IInterface> sut, IInterface item )
		{
			Assert.Equal( sut.Object, item );
		}
	}
}
