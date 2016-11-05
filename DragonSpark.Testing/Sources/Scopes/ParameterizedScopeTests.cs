using DragonSpark.Sources.Scopes;
using Xunit;

namespace DragonSpark.Testing.Sources.Scopes
{
	public class ParameterizedScopeTests
	{
		[Fact]
		public void VerifyConfiguration()
		{
			const int parameter = 123;
			var before = Configurable.Default.Get( parameter );
			Assert.Equal( parameter + 6776, before );

			Configurable.Default.Assign( () => i => i * 2 );

			const int answer = 42;
			var after = Configurable.Default.Get( answer );
			Assert.Equal( answer * 2, after );
		}

		[Fact]
		public void Coverage()
		{
			var sut = new ParameterizedScope<object, object>();
			Assert.Null( sut.Get( new object() ) );
		}

		sealed class Configurable : ParameterizedScope<int, int>
		{
			public static Configurable Default { get; } = new Configurable();
			Configurable() : base( i => i + 6776 ) {}
		}
	}
}