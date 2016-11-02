using DragonSpark.Application;
using DragonSpark.Sources;
using DragonSpark.Testing.Objects.FileSystem;
using DragonSpark.Windows.Runtime;
using System.Linq;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class AllPartsTests
	{
		[Fact]
		public void Coverage()
		{
			InitializePartsCommand.Default.Execute();
			Assert.True( AllParts.Default.Get( ApplicationAssemblies.Default.Unwrap() ).Any() );
		}
	}
}