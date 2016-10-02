using JetBrains.Annotations;
using System;
using System.Collections.Immutable;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class AutoGuardAspectTests
	{
		[Fact]
		public void Basic()
		{
			Assert.Throws<ArgumentException>( "item", () => new Testing( default(ImmutableArray<object>) ) );
		}

		class Testing
		{
			public Testing( [UsedImplicitly]ImmutableArray<object> item ) {}
		}
	}
}
