using System.Collections.Generic;
using DragonSpark.Extensions;
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
		public void ExecuteOn()
		{
			var called = false;
			dictionary.ExecuteOn( "Key1", s => called = true );
			Assert.True( called );
		}

		[Fact]
		public void ExecuteOnNot()
		{
			var called = false;
			dictionary.ExecuteOn( "Key10", s => called = true );
			Assert.False( called );
		}

		[Fact]
		public void Ensure()
		{
			var sut = new Dictionary<string, int>();
			var called = 0;
			Assert.Equal( 1, sut.Ensure( "Key", x => ++called ) );
			Assert.Equal( 1, sut.Ensure( "Key", x => ++called ) );
		}
	}
}