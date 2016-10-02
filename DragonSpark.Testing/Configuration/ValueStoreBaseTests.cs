using DragonSpark.Configuration;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Testing.Configuration
{
	public class ValueStoreBaseTests
	{
		[Theory, AutoData]
		void CheckEquality( ValueStore sut, object value, string secondKey, Guid id, int number )
		{
			Assert.Empty( sut );

			var key = id.ToString();
			var composite = new Registration( key, value, secondKey );
			sut.Add( composite );
			sut.Add( new Registration( "asdfasdf", number ) );

			var first = sut.Get( key );
			Assert.NotNull( first );
			Assert.Same( first, value );

			var second = sut.Get( secondKey );
			Assert.NotNull( second );
			Assert.Same( second, value );

			Assert.Same( first, second );

			var third = sut.Get( "asdfasdf" );
			Assert.Equal( number, third );
		}

		class ValueStore : ValueStoreBase {}
	}
}