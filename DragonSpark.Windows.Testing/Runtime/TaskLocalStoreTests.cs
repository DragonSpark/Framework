using DragonSpark.Windows.Runtime;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class TaskLocalStoreTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Assign( int number, TaskLocalStore<int> sut )
		{
			sut.Assign( number );
			Assert.Equal( number, sut.Get() );
		}
	}
}