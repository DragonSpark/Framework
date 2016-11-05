using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class DelegatedDisposableTests
	{
		[Fact]
		public void Verify()
		{
			var called = false;
			var sut = new DelegatedDisposable( () => called = true );
			Assert.False( called );
			sut.Dispose();
			Assert.True( called );
		}
	}
}