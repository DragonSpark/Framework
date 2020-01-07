using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Commands
{
	public class DecoratedCommandTests
	{
		[Fact]
		public void Verify()
		{
			var count = 0;
			var inner = new Command<None>(x => count++);
			var sut   = new Command<None>(inner);
			sut.Execute();
			count.Should()
			     .Be(1);
		}
	}
}