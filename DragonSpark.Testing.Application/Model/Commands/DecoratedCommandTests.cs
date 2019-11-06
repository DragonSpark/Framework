using FluentAssertions;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime;
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