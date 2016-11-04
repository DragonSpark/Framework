using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class AssemblyResourcePathSelectorTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.Empty( AssemblyResourcePathSelector.DefaultImplementation.Implementation.GetEnumerable( GetType().AssemblyQualifiedName ) );
		}
	}
}