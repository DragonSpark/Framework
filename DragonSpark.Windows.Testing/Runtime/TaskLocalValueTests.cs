using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Runtime;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class TaskLocalValueTests
	{
		[Theory, MoqAutoData]
		public void Assign( int number, TaskLocalValue<int> sut )
		{
			sut.Assign( number );
			Assert.Equal( number, sut.Item );
		}
	}
}