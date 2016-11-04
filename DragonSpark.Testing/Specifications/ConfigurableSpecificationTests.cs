using DragonSpark.Application;
using DragonSpark.Specifications;
using System;
using Xunit;

namespace DragonSpark.Testing.Specifications
{
	public class ConfigurableSpecificationTests
	{
		[Fact]
		public void Coverage()
		{
			var sut = new ConfigurableSpecification<DateTimeOffset>( o => parameter => parameter.Date == DateTime.Today );
			Assert.False( sut.IsSatisfiedBy( Clock.Default.Get() )  );
		}
	}
}