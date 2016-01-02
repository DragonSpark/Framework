using DragonSpark.Activation;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public class IExecutionContextTests
	{
		[Fact]
		public void Item()
		{
			Assert.Equal( typeof(object), ExecutionContext.Instance.Item.GetType() );
		} 
	}
}