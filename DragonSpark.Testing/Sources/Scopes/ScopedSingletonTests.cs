using DragonSpark.Sources.Scopes;
using Xunit;

namespace DragonSpark.Testing.Sources.Scopes
{
	public class ScopedSingletonTests
	{
		[Fact]
		public void CachingTests()
		{
			Assert.Same( Scope.Default.Get(), Scope.Default.Get() );
		}

		class Scope : ScopedSingleton<object>
		{
			public static Scope Default { get; } = new Scope();
			Scope() : base( () => new object() ) {}
		}
	}
}