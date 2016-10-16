using DragonSpark.Sources;
using Xunit;

namespace DragonSpark.Testing.Sources
{
	public class SuppliedSourceTests
	{
		[Fact]
		public void Dispose()
		{
			var reference = new object();
			var sut = new SuppliedSource<object>( reference );
			Assert.Same( reference, sut.Get() );
			sut.Dispose();
			Assert.Null( sut.Get() );
		}
	}
}