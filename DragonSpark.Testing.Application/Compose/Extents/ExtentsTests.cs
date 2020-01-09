using DragonSpark.Compose;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Application.Compose.Extents
{
	public sealed class ExtentsTests
	{
		[Fact]
		void VerifyCondition()
		{
			var parameter = new object();
			Start.An.Extent.Of.Any.Into.Condition.Always.Get()
			     .Get(parameter)
			     .Should()
			     .BeTrue();
		}
	}
}