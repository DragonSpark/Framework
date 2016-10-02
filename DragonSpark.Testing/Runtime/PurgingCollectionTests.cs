using DragonSpark.Runtime;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class PurgingCollectionTests
	{
		[Fact]
		public void Array()
		{
			var items = new[] { 1, 2, 3, 5 };
			var sut = new PurgingCollection<int>( items.ToList() );
			Assert.Equal( sut.ToArray(), items );
			Assert.Empty( sut );
		}

		[Fact]
		public void Enumerable()
		{
			var items = new[] { 1, 2, 3, 5 };
			var sut = new PurgingCollection<int>( items.ToList() );
			Assert.Equal( sut.ToImmutableArray(), items );
			Assert.Empty( sut );
		}
	}
}