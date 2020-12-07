using DragonSpark.Runtime.Invocation.Expressions;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime.Invocation.Expressions
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