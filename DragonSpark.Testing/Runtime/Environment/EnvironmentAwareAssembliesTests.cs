using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Runtime.Environment;
using DragonSpark.Testing.Environment;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime.Environment
{
	public sealed class EnvironmentAwareAssembliesTests
	{
		[Fact]
		public void Verify()
		{
			EnvironmentAwareAssemblies.Default.Get("Production")
			                          .Open()
			                          .Should()
			                          .Equal(typeof(HelloWorld).Assembly,
			                                 typeof(EnvironmentAwareAssembliesTests).Assembly,
			                                 typeof(XunitTestingApplicationAttribute).Assembly);
		}

		[Fact]
		public void VerifyDevelopment()
		{
			EnvironmentAwareAssemblies.Default.Get("Development")
			                          .Open()
			                          .Should()
			                          .Equal(typeof(Testing.Environment.Development.HelloWorld).Assembly,
			                                 typeof(HelloWorld).Assembly,
			                                 typeof(EnvironmentAwareAssembliesTests).Assembly,
			                                 typeof(XunitTestingApplicationAttribute).Assembly);
		}
	}
}