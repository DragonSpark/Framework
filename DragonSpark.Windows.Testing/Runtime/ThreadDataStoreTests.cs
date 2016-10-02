using DragonSpark.Windows.Runtime;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ThreadDataStoreTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Assign( ThreadDataStore<int> sut, int number )
		{
			sut.Assign( number );
			Assert.Equal( number, sut.Get() );
		} 
	}
}