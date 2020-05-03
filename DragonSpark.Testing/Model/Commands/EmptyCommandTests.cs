using DragonSpark.Model.Commands;
using Xunit;

namespace DragonSpark.Testing.Model.Commands
{
	public class EmptyCommandTests
	{
		[Fact]
		public void Coverage() => EmptyCommand<object>.Default.Execute(null!);
	}
}