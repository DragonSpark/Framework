using DragonSpark.TypeSystem.Metadata;
using Xunit;

namespace DragonSpark.Testing.TypeSystem.Metadata
{
	public class AttributeSupportTests
	{
		[Fact]
		public void All()
		{
			Assert.True( AttributeSupport<PriorityAttribute>.All.Contains( typeof(Extended) ) );
			Assert.False( AttributeSupport<PriorityAttribute>.Local.Contains( typeof(Extended) ) );
		}

		class Extended : BaseClass {}

		[Priority( Priority.AfterHigh )]
		class BaseClass {}
	}
}