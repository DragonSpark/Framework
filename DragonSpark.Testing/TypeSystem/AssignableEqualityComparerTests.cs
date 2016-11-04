using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class AssignableEqualityComparerTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.Equal( new AssignableEqualityComparer().GetHashCode( GetType() ), GetType().GetHashCode() );
		}
	}
}