using FluentAssertions;
using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Application.Reflection
{
	public class ActivationDelegatesTests
	{
		[Fact]
		public void Coverage()
		{
			Delegates.Empty.Should()
			         .BeSameAs(Delegates.Empty);
			Delegates.Empty();
		}

		[Fact]
		public void CoverageGeneric()
		{
			Delegates<object>.Empty.Should()
			                 .BeSameAs(Delegates<object>.Empty);
			Delegates<object>.Empty(null);
		}
	}
}