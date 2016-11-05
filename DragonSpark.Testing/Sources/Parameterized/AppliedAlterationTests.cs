using DragonSpark.Sources.Parameterized;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized
{
	public class AppliedAlterationTests
	{
		[Fact]
		public void Verify()
		{
			var instance = new object();
			var sut = new AppliedAlteration<object>( parameter => null );
			Assert.Same( instance, sut.Get( instance ) );
		}
	}
}