using DragonSpark.Application;
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
			Assert.True( AllParts.Default.Get( ApplicationAssemblies.Default.Get().ToArray() ).Any() );
		}
	}
}