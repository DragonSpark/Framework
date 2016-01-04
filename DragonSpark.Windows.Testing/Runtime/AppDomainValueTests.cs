using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Runtime;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class AppDomainValueTests
	{
		[Theory, ConfiguredMoqAutoData]
		public void Assign( AppDomainValue<int> sut, int number )
		{
			sut.Assign( number );
			Assert.Equal( number, sut.Item );
		}
	}
}