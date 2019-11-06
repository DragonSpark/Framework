using System.Threading.Tasks;
using FluentAssertions;
using DragonSpark.Operations;
using Xunit;

namespace DragonSpark.Testing.Application.Operations
{
	public class OperationTests
	{
		sealed class Number : Operation<string, uint>
		{
			public static Number Default { get; } = new Number();

			Number() : base(x => new ValueTask<uint>((uint)x.Length)) {}
		}

		[Fact]
		Task Verify() => Number.Default.Then()
		                       .Then(x => x * 2)
		                       .Get()
		                       .Get("Hello World!")
		                       .ContinueWith(x => x.Result.Should().Be(24));
	}
}