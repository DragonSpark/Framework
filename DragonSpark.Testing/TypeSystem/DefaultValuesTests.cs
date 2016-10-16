using DragonSpark.TypeSystem;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class DefaultValuesTests
	{
		[Fact]
		public void Empty()
		{
			var sut = DefaultValues.Default.Get( typeof(IEnumerable<int>) );
			Assert.Equal( Enumerable.Empty<int>(), sut );
		}
	}
}