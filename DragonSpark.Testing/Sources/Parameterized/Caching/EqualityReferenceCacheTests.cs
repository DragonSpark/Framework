using DragonSpark.Sources.Parameterized.Caching;
using System;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class EqualityReferenceCacheTests
	{
		[Fact]
		public void Contains()
		{
			var sut = new EqualityReferenceCache<MyClass,object>( o => new object() );
			var one = new MyClass( 4 );
			var two = new MyClass( 4 );
			Assert.Same( sut.Get( one ), sut.Get( two ) );
			Assert.True( sut.Contains( new MyClass( 4 ) ) );
			Assert.True( sut.Remove( new MyClass( 4 ) ) );
			Assert.False( sut.Contains( new MyClass( 4 ) ) );
			sut.Set( new MyClass( 4 ), new object() );
			Assert.True( sut.Contains( new MyClass( 4 ) ) );
		}

		class MyClass : IEquatable<MyClass>
		{
			public MyClass( int number )
			{
				Number = number;
			}

			public int Number { get; }

			public bool Equals( MyClass other ) => !ReferenceEquals( null, other ) && ( ReferenceEquals( this, other ) || Number == other.Number );

			public override bool Equals( object obj ) => !ReferenceEquals( null, obj ) && ( ReferenceEquals( this, obj ) || obj.GetType() == GetType() && Equals( (MyClass)obj ) );

			public override int GetHashCode() => Number;

			public static bool operator ==( MyClass left, MyClass right ) => Equals( left, right );

			public static bool operator !=( MyClass left, MyClass right ) => !Equals( left, right );
		}
	}
}