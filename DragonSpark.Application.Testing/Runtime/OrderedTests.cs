using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Application.Testing.Runtime;

public sealed class OrderedTests
{
	[Fact]
	public void Verify()
	{
		var subjects = new[] { new Subject(), new Subject { Order = 10 }, new Subject(), }.Result();
		var array    = subjects.Open().OrderedLarge().ToList();
		array[0].Order.Should().Be(0);
		array[1].Order.Should().Be(10);
		array[2].Order.Should().Be(1);
	}

	sealed class Subject : ILargeOrderAware
	{
		public uint? Order { get; set; }
	}
}