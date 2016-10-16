using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class CollectionBaseTests
	{
		[Fact]
		public void Coverage()
		{
			var sut = new Collection();

		}

		sealed class Collection : CollectionBase<object>
		{}
	}
}