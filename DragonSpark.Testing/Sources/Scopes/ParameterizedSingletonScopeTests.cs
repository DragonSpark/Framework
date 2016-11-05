using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using Xunit;

namespace DragonSpark.Testing.Sources.Scopes
{
	public class ParameterizedSingletonScopeTests
	{
		[Fact]
		public void Coverage()
		{
			var instance = new object();
			Assert.Same( instance, new SingletonScope<object>( instance ).Get() );

			var sut = new SingletonScope<object>();
			Assert.Null( sut.Get() );
			sut.Assign( instance.Self );

			Assert.Same( instance, sut.Get() );

			Assert.Null( new ParameterizedSingletonScope<object, object>().Get( new object() ) );
		}
	}
}