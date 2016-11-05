using DragonSpark.Sources.Scopes;
using Xunit;

namespace DragonSpark.Testing.Sources.Scopes
{
	public class SingletonScopeTests
	{
		[Fact]
		public void CachingTests()
		{
			Assert.Same( Scope.Default.Get(), Scope.Default.Get() );
		}

		[Fact]
		public void Coverage()
		{
			var instance = new object();
			Assert.Same( instance, new ParameterizedSingletonScope<object, object>( instance ).Get( this ) );
		}

		class Scope : SingletonScope<object>
		{
			public static Scope Default { get; } = new Scope();
			Scope() : base( () => new object() ) {}
		}
	}
}