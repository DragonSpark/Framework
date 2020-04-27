using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Collections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Collections
{
	public class HasTests
	{
		[Fact]
		public void Verify()
		{
			var item = new object();
			var items = item.Yield()
			                .ToList();
			new Has<object>(items).Get(item).Should().BeTrue();
		}
	}
}