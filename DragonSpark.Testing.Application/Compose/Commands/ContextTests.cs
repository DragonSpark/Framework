using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Xunit;

namespace DragonSpark.Testing.Application.Compose.Commands
{
	public sealed class ContextTests
	{
		[Fact]
		void Verify()
		{
			Start.A.Command.Of.Any.By.Empty.Should().BeSameAs(EmptyCommand<object>.Default);
		}

		[Fact]
		void VerifyCalling()
		{
			var count = 0;
			Start.A.Command.Of.None.By.Calling(_ => count++).Execute();

			count.Should().Be(1);
		}
	}
}