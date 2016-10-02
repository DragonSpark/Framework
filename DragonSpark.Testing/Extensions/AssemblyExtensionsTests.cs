using DragonSpark.Extensions;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class AssemblyExtensionsTests
	{
		[Fact]
		public void GetRootNamespace()
		{
			var result = GetType().Assembly.GetRootNamespace();
			Assert.Equal( "DragonSpark.Testing", result );
		}
	}
}
