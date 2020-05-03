using DragonSpark.Runtime;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Commands
{
	public class DelegatesTests
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
			Delegates<object>.Empty(null!);
		}
	}
}