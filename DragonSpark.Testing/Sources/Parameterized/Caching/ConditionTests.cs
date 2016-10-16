using DragonSpark.Sources.Parameterized.Caching;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class ConditionTests
	{
		[Fact]
		public void Default()
		{
			var key = new object();
			Assert.False( Condition<object>.Default.Get( key ).IsApplied );
			Condition<object>.Default.Get( key ).Apply();
			Assert.True( Condition<object>.Default.Get( key ).IsApplied );
		}
	}
}