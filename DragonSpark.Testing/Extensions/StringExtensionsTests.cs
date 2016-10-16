using DragonSpark.Extensions;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class StringExtensionsTests
	{
		[Fact]
		public void Capitalized()
		{
			var temp = "asdf";
			Assert.Equal( "Asdf", temp.Capitalized() );
		}

		[Fact]
		public void ToStringArray()
		{
			var items = "this; is; a; test".ToStringArray();
			Assert.Equal( 4, items.Length );
		}
	}
}