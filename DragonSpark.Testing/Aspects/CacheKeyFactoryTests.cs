using DragonSpark.Aspects;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class CacheKeyFactoryTests
	{
		[Theory, AutoData]
		public void EnsureSame( CacheKeyFactory sut )
		{
			var first = sut.Create( new object[] { typeof(Class), typeof(CacheKeyFactoryTests).GetMethod( nameof(EnsureSame) ), true } );
			var second = sut.Create( new object[] { typeof(Class), typeof(CacheKeyFactoryTests).GetMethod( nameof(EnsureSame) ), true } );
			Assert.Equal( first, second );
		}

		[Theory, AutoData]
		public void EnsureDifferent( CacheKeyFactory sut )
		{
			var first = sut.Create( new object[] { typeof(Class), typeof(CacheKeyFactoryTests).GetMethod( nameof(EnsureSame) ), false } );
			var second = sut.Create( new object[] { typeof(Class), typeof(CacheKeyFactoryTests).GetMethod( nameof(EnsureSame) ), true } );
			Assert.NotEqual( first, second );
		}
	}
}