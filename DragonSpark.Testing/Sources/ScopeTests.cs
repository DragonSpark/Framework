using DragonSpark.Sources;
using Xunit;

namespace DragonSpark.Testing.Sources
{
	public class ScopeTests
	{
		[Fact]
		public void CachingTests()
		{
			Assert.Same( Scope.Default.Get(), Scope.Default.Get() );
		}

		class Scope : Scope<object>
		{
			public static Scope Default { get; } = new Scope();
			Scope() : base( Factory.Cache( () => new object() ) ) {}
		}
	}
}