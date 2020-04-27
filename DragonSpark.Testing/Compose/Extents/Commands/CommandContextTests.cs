using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Compose.Extents.Commands
{
	public sealed class CommandContextTests
	{
		[Fact]
		public void Verify()
		{
			Start.A.Command.Of.Any.By.Empty.Get().Should().BeSameAs(EmptyCommand<object>.Default);
		}

		[Fact]
		public void VerifyCalling()
		{
			var count = 0;
			Start.A.Command.Of.None.By.Calling(_ => count++).Get().Execute();

			count.Should().Be(1);
		}
	}
}