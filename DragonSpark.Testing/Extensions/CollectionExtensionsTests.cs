using DragonSpark.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class CollectionExtensionsTests
	{
		[Fact]
		public void AddRange()
		{
			ICollection<int> list = new List<int>();
			var enumerable = new []{ 3, 4 };
			list.AddRange( enumerable );
			Assert.Equal( enumerable.ToImmutableHashSet(), list.ToImmutableHashSet() );
		}
	}
}