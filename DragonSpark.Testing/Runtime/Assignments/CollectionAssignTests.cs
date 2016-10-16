using DragonSpark.Runtime.Assignments;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Runtime.Assignments
{
	public class CollectionAssignTests
	{
		[Fact]
		public void Verify()
		{
			var list = new List<object>();
			var item = new object();
			using ( list.Assignment( item ) )
			{
				Assert.Contains( item, list );
			}

			Assert.DoesNotContain( item, list );
		}
	}
}