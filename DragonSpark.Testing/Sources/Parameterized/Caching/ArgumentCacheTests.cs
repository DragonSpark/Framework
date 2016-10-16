﻿using DragonSpark.Sources.Parameterized.Caching;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class ArgumentCacheTests
	{
		[Fact]
		public void Set()
		{
			var sut = new ArgumentCache<object, object>();
			var key = new object();
			Assert.Null( sut.Get( key ) );
			var o = new object();
			sut.Set( key, o );
			Assert.Same( o, sut.Get( key ) );
			sut.Remove( key );
			Assert.Null( sut.Get( key ) );
		}
	}
}