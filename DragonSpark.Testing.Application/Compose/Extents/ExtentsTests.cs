using FluentAssertions;
using DragonSpark.Compose;
using Xunit;

namespace DragonSpark.Testing.Application.Compose.Extents
{
	public sealed class ExtentsTests
	{
		[Fact]
		void VerifyCondition()
		{
			var parameter = new object();
			Start.An.Extent.Of.Any.Into.Condition.Always
			     .Get(parameter)
			     .Should()
			     .BeTrue();
		}
	}
}