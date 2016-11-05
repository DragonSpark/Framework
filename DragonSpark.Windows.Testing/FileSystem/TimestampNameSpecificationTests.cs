using DragonSpark.Windows.FileSystem;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class TimestampNameSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			Assert.True( TimestampNameSpecification.Default.IsSatisfiedBy( "1976-06-07--23-17-57" ) );
			Assert.False( TimestampNameSpecification.Default.IsSatisfiedBy( "adds-06-07--23-17-57" ) );
		}
	}
}