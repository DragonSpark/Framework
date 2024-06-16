using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Model.Operations;

public class OperationTests
{
	sealed class Number : Selecting<string, uint>
	{
		public static Number Default { get; } = new();

		Number() : base(x => new ValueTask<uint>((uint)x.Length)) {}
	}

	[Fact]
	Task Verify() => Number.Default.Then()
	                       .Select(x => x * 2)
	                       .Allocate()
	                       .Get()
	                       .Get("Hello World!")
	                       .ContinueWith(async x => (await x).Should().Be(24));
}