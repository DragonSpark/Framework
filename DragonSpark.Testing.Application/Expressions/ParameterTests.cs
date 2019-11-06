using FluentAssertions;
using DragonSpark.Runtime.Invocation.Expressions;
using Xunit;

namespace DragonSpark.Testing.Application.Expressions
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