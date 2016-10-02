using DragonSpark.Runtime;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class KeyFactoryTests
	{
		[Theory, AutoData]
		public void EnsureSame( KeyFactory sut )
		{
			var first = KeyFactory.CreateUsing( typeof(Class), typeof(KeyFactoryTests).GetMethod( nameof(EnsureSame) ), true );
			var second = KeyFactory.CreateUsing( typeof(Class), typeof(KeyFactoryTests).GetMethod( nameof(EnsureSame) ), true );
			Assert.Equal( first, second );
		}

		[Theory, AutoData]
		public void EnsureDifferent( KeyFactory sut )
		{
			var first = KeyFactory.CreateUsing( typeof(Class), typeof(KeyFactoryTests).GetMethod( nameof(EnsureSame) ), false );
			var second = KeyFactory.CreateUsing( typeof(Class), typeof(KeyFactoryTests).GetMethod( nameof(EnsureSame) ), true );
			Assert.NotEqual( first, second );
		}
	}
}