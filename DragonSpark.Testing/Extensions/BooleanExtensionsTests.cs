using DragonSpark.Extensions;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class BooleanExtensionsTests
	{
		[Fact]
		public void IsTrue()
		{
			var called = false;
			true.IsTrue( () => called = true );
			Assert.True( called );
		}

		[Fact]
		public void IsFalse()
		{
			var called = false;
			false.IsFalse( () => called = true );
			Assert.True( called );

			called = false;
			true.IsFalse( () => called = true );
			Assert.False( called );
		}

		[Fact]
		public void And()
		{
			var called = false;
			true.And( true ).IsTrue( () => called = true );
			Assert.True( called );
			
			called = false;
			true.And( false ).IsTrue( () => called = true );
			false.And( true ).IsTrue( () => called = true );
			Assert.False( called );
		}
	}
}