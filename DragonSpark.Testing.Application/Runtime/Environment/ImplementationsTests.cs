using FluentAssertions;
using DragonSpark.Runtime.Environment;
using DragonSpark.Runtime.Execution;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class ImplementationsTests
	{
		[Fact]
		void Verify()
		{
			Implementations<object>.Store.Get().Should().NotBeSameAs(Implementations<object>.Store.Get());

			SystemStores<object>.Default.Get().Should().BeOfType<Logical<object>>();
		}
	}
}