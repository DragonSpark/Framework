using DragonSpark.Application;
using DragonSpark.Aspects;
using DragonSpark.Testing.Framework;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Aspects
{
	public class FreezeAttributeTests : TestCollectionBase
	{
		public FreezeAttributeTests( ITestOutputHelper output ) : base( output ) {}

		[Fact]
		public void ProviderCached()
		{
			var sut = new Source();

			Assert.Equal( 1, sut.Cached );
			Assert.Equal( 1, sut.Cached );
		}

		[Fact]
		public void CheckFreeze()
		{
			var sut = new Disposable();
			sut.Dispose();
			Assert.Equal( 1, sut.Count );
			sut.Dispose();
			Assert.Equal( 1, sut.Count );

			sut.Other();
			Assert.Equal( 2, sut.Count );
			sut.Other();
			Assert.Equal( 2, sut.Count );
		}

		public class Disposable : IDisposable
		{
			public int Count { get; private set; }
			

			public void Dispose() => Dispose( true );

			public void Other() => Dispose( false );

			[Freeze]
			protected virtual void Dispose( bool disposing ) => Count++;
		}

		public class Source : SuppliedTypeSource
		{
			public int Count { get; private set; }

			[Freeze]
			public int Cached => ++Count;

			
		}

		[Theory, AutoData]
		public void BasicCache( CacheItem sut )
		{
			Assert.Equal( 1, sut.Count );
			sut.Up();
			Assert.Equal( 2, sut.Count );
			sut.Up();
			Assert.Equal( 2, sut.Count );

			sut.UpWith( 2 );
			Assert.Equal( 4, sut.Count );
			sut.UpWith( 2 );
			Assert.Equal( 4, sut.Count );
			sut.UpWith( 3 );
			Assert.Equal( 7, sut.Count );
			sut.UpWith( 3 );
			Assert.Equal( 7, sut.Count );
		}

		public class CacheItem
		{
			public int Count { get; private set; } = 1;

			[Freeze]
			public void Up()
			{
				Count++;
			}

			[Freeze]
			public void UpWith( int i )
			{
				Count += i;
			}
		}
	}
}