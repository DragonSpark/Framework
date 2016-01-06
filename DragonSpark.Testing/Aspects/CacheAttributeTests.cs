using System;
using DragonSpark.Aspects;
using DragonSpark.TypeSystem;
using Ploeh.AutoFixture.Xunit2;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class CacheAttributeTests
	{
		[Fact]
		public void ProviderCached()
		{
			var sut = new Source();

			sut.Create();
			Assert.Equal( 1, sut.Count );
			sut.Create();
			Assert.Equal( 1, sut.Count );

			Assert.Equal( 2, sut.Cached );
			Assert.Equal( 2, sut.Cached );
		}

		public class Source : AssemblySourceBase
		{
			public int Count { get; private set; }

			[Cache]
			public int Cached => ++Count;

			protected override Assembly[] CreateItem()
			{
				Count++;
				return new Assembly[0];
			}
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

			[Cache]
			public void Up()
			{
				Count++;
			}

			[Cache]
			public void UpWith( int i )
			{
				Count += i;
			}
		}
	}
}