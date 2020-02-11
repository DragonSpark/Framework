using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Commands
{
	public class CompositeCommandTests
	{
		[Fact]
		public void Verify()
		{
			var count = 0;
			new CompositeCommand<None>(new Command<None>(x => count++), new Command<None>(x => count++))
				.Execute();
			count.Should()
			     .Be(2);
		}
	}
}