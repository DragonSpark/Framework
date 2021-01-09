using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Model.Operations
{
	public class OperationTests
	{
		sealed class Number : Selecting<string, uint>
		{
			public static Number Default { get; } = new Number();

			Number() : base(x => new ValueTask<uint>((uint)x.Length)) {}
		}

		[Fact]
		Task Verify() => Number.Default.Then()
		                       .Select(x => x * 2)
		                       .Allocate()
		                       .Get()
		                       .Get("Hello World!")
		                       .ContinueWith(x => x.Result.Should().Be(24));
	}
}