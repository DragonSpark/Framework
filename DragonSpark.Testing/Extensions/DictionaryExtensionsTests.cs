using DragonSpark.Extensions;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class DictionaryExtensionsTests
	{
		readonly Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ "Key1", "Value1" },
			{ "Key2", "Value2" },
			{ "Key3", "Value3" }
		};
		
		[Fact]
		public void TryGet()
		{
			Assert.NotNull( dictionary.TryGet( "Key1" ) );
			Assert.Null( dictionary.TryGet( "Key10" ) );
			Assert.Equal( "DefaultValue", dictionary.TryGet( "Key10", () => "DefaultValue" ) );
		}

		[Fact]
		public void Ensure()
		{
			var sut = new Dictionary<string, int>();
			var called = 0;
			Assert.Equal( 1, sut.Ensure( "Key", x => ++called ) );
			Assert.Equal( 1, sut.Ensure( "Key", x => ++called ) );
		}

		[Fact]
		public void Assignment()
		{
			var sut = new Dictionary<int, int>();
			Assert.False( sut.ContainsKey( 4 ) );
			using ( sut.Assignment( 4, 5 ) )
			{
				Assert.True( sut.ContainsKey( 4 ) );
				Assert.Equal( 5, sut[4] );
			}
			
			Assert.False( sut.ContainsKey( 4 ) );
		}
	}
}