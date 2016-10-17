using DragonSpark.Windows.Setup;
using System;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup
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