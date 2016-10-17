using DragonSpark.Commands;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Setup;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup
{
	public class ConsoleApplicationTests
	{
		[Fact]
		public void Coverage() => new ConsoleApplication().Run( Items<string>.Default );
	}
}