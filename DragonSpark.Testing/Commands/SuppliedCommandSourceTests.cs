using DragonSpark.Commands;
using Xunit;

namespace DragonSpark.Testing.Commands
{
	public class SuppliedCommandSourceTests
	{
		[Fact]
		public void Coverage()
		{
			var command = new Command();
			var source = new SuppliedCommandSource( command );
			Assert.Contains( command, source );
		}
	}
}