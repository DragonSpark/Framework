using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Runtime;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ThreadLocalValueTests
	{
		[Theory, ConfiguredMoqAutoData]
		public void Assign( ThreadLocalValue<int> sut, int number )
		{
			sut.Assign( number );
			Assert.Equal( number, sut.Item );
		} 
	}
}