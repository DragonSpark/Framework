using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class ValueTupleTests
	{
		[Fact]
		public void Equality()
		{
			var one = ValueTuple.Create( 123, true );
			var two = ValueTuple.Create( 123, true );
			Assert.True( one == two );
			Assert.False( one != two );
			Assert.Equal( one.GetHashCode(), two.GetHashCode() );
			Assert.True( one.Equals( (object)two ) );
			Assert.False( one.Equals( new object() ) );
		}

		[Fact]
		public void EqualityTriple()
		{
			var one = ValueTuple.Create( 123, true, "HelloWorld!" );
			var two = ValueTuple.Create( 123, true, "HelloWorld!" );
			Assert.True( one == two );
			Assert.False( one != two );
			Assert.Equal( one.GetHashCode(), two.GetHashCode() );
			Assert.True( one.Equals( (object)two ) );
			Assert.False( one.Equals( new object() ) );
		}
	}
}