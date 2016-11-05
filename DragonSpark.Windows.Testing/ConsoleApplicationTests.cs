using DragonSpark.Commands;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Windows.Testing
{
	public class ConsoleApplicationTests
	{
		[Fact]
		public void Coverage() => new ConsoleApplication().Run( Items<string>.Default );
	}
}