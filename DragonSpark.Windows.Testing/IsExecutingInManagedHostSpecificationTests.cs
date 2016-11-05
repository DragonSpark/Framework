using System;
using Xunit;

namespace DragonSpark.Windows.Testing
{
	public class IsExecutingInManagedHostSpecificationTests
	{
		[Fact]
		public void Verify()
		{
			Assert.False( IsExecutingInManagedHostSpecification.Default.IsSatisfiedBy( AppDomain.CurrentDomain ) );
		}
	}
}