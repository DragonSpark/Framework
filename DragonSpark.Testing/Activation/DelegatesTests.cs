using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System;
using System.Windows.Media.Animation;
using Xunit;

// ReSharper disable AccessToModifiedClosure

namespace DragonSpark.Testing.Activation
{
	public class DelegatesTests
	{
		[Fact]
		public void CachedSource()
		{
			var count = 0;
			var factory = new Func<int>( () => ++count );
			var singleton = new SuppliedDeferredSource<int>( factory );
			Assert.Equal( 1, singleton.Get() );
			Assert.Equal( 1, singleton.Get() );
			Assert.Equal( 1, singleton.Get() );
		}

		[Fact]
		public void CachedFactory()
		{
			var count = 0;
			var factory = new Func<int>( () => ++count );

			var cached = SingletonDelegateBuilder<int>.Default.Get( factory );
			Assert.Equal( 0, count );
			Assert.Equal( 1, cached() );
			Assert.Equal( 1, cached() );
			Assert.Equal( 1, cached() );
			Assert.Equal( 1, count );
		}

		[Fact]
		public void ParameterizedScopeAssignment()
		{
			var count = 0;
			var one = new object().Sourced();
			
			var scope = new ParameterizedScope<int, int>( i => i * ++count ).ScopedWith( one );

			Assert.Equal( 0, count );
			Assert.Equal( 2, scope.Get( 2 ) );
			Assert.Equal( 1, count );
			Assert.Equal( 4, scope.Get( 2 ) );
			Assert.Equal( 2, count );
			Assert.Equal( 6, scope.Get( 2 ) );
			Assert.Equal( 3, count );

			var two = new object().Sourced();
			scope.Assign( two );
			scope.Assign( i => i * ++count * -1 );
			count = 0;

			Assert.Equal( 0, count );
			Assert.Equal( -2, scope.Get( 2 ) );
			Assert.Equal( 1, count );
			Assert.Equal( -4, scope.Get( 2 ) );
			Assert.Equal( 2, count );
			Assert.Equal( -6, scope.Get( 2 ) );
			Assert.Equal( 3, count );

			scope.Assign( one );
			count = 0;

			Assert.Equal( 0, count );
			Assert.Equal( 2, scope.Get( 2 ) );
			Assert.Equal( 1, count );
			Assert.Equal( 4, scope.Get( 2 ) );
			Assert.Equal( 2, count );
			Assert.Equal( 6, scope.Get( 2 ) );
			Assert.Equal( 3, count );
		}

		[Fact]
		public void ParameterizedScopeCache()
		{
			var count = 0;
			var one = new object().Sourced();
			
			var scope = new ParameterizedScope<Type, int>( type => type.Name.Length * ++count ).ScopedWith( one );
			var cache = scope.ToCache();
			var first = typeof(int).Name.Length;
			Assert.Equal( first, cache.Get( typeof(int) ) );
			Assert.Equal( 1, count );
			Assert.Equal( first, cache.Get( typeof(int) ) );
			Assert.Equal( 1, count );

			var two = new object().Sourced();
			scope.Assign( two );
			scope.Assign( type => type.Name.Length + 5000 * ++count );
			count = 0;

			var t = typeof(DateTimeOffset);
			var second = t.Name.Length + 5000;
			Assert.Equal( second, cache.Get( t ) );
			Assert.Equal( 1, count );
			Assert.Equal( second, cache.Get( t ) );
			Assert.Equal( 1, count );

			scope.Assign( one );
			count = 10;

			Assert.Equal( first, cache.Get( typeof(int) ) );
			Assert.Equal( 10, count );
			Assert.Equal( first, cache.Get( typeof(int) ) );
			Assert.Equal( 10, count );

			var thirdType = typeof(StringAnimationUsingKeyFrames);
			var secondOne = thirdType.Name.Length * ( count + 1 );
			Assert.Equal( secondOne, cache.Get( thirdType ) );
			Assert.Equal( 11, count );
			Assert.Equal( secondOne, cache.Get( thirdType ) );
			Assert.Equal( 11, count );

			Assert.Equal( second, cache.Get( t ) );
			Assert.Equal( 11, count );
			Assert.Equal( second, cache.Get( t ) );
			Assert.Equal( 11, count );
		}
	}
}