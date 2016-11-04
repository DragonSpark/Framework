using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class ExtensionsTests
	{
		[Fact]
		public void IsInstanceOfType()
		{
			Assert.False( GetType().Yield().AsAdapters().IsInstanceOfType( "Hello World!" ) );
			Assert.True( GetType().Yield().AsAdapters().AsEnumerable().IsInstanceOfType( this ) );
			Assert.True( this.Yield().AsAdapters().AsEnumerable().IsInstanceOfType( this ) );
			Assert.True( GetType().Yield().IsInstanceOfType( this ) );
			Assert.Contains( GetType(), this.Yield().AsTypes() );
			Assert.Contains( GetType(), this.Yield().AsAdapters().AsEnumerable().SelectTypes() );
			Assert.True( this.Yield().AsAdapters().AsEnumerable().IsAssignableFrom( GetType() ) );
		}
	}
}