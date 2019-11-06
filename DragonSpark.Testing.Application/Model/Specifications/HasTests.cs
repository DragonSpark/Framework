using System.Linq;
using FluentAssertions;
using DragonSpark.Model.Sequences.Collections;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Specifications
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