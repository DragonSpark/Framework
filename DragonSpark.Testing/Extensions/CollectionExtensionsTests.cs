using DragonSpark.Extensions;
using System;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class CollectionExtensionsTests
	{
		[Fact]
		public void AddRange()
		{
			// Assert.Throws<ArgumentNullException>( () => new Collection().AddRange( null ) );
			Assert.Throws<ArgumentNullException>( () => CollectionExtensions.AddRange( null, new[] { 1 } ) );
		} 
	}
}