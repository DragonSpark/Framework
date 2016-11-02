using DragonSpark.Application;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Objects.FileSystem;
using DragonSpark.TypeSystem;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class AllPartsTests
	{
		[Fact]
		public void Before() => Assert.Empty( ApplicationAssemblies.Default.Unwrap() );

		[Fact]
		public void Coverage()
		{
			Assert.Empty( ApplicationAssemblies.Default.Unwrap() );
			InitializePartsCommand.Default.Execute();
			Assert.Empty( ApplicationAssemblies.Default.Unwrap() );
			Assert.True( AllParts.Default.Get( GetType().Assembly ).Any() );
		}

		[Fact]
		public void ZorderAfter() => Assert.Empty( ApplicationAssemblies.Default.Unwrap() );
	}
}