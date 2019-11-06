using FluentAssertions;
using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Runtime.Environment;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class HostingAssemblyTests
	{
		[Fact]
		void Verify()
		{
			HostingAssembly.Default.Get().Should().BeSameAs(typeof(XunitTestingApplicationAttribute).Assembly);
		}
	}
}