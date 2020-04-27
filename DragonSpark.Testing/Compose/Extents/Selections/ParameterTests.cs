using DragonSpark.Runtime.Invocation.Expressions;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Compose.Extents.Selections
{
	public class ParameterTests
	{
		[Fact]
		public void Coverage()
		{
			Parameter.Default.Should()
			         .BeSameAs(Parameter.Default);
		}
	}
}