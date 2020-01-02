using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Runtime.Environment;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class EnvironmentAwareAssembliesTests
	{
		[Fact]
		void Verify()
		{
			EnvironmentAwareAssemblies.Default.Get("Production")
			              .Get()
			              .Open()
			              .Should()
			              .Equal(typeof(Testing.Environment.HelloWorld).Assembly,
			                     typeof(EnvironmentAwareAssembliesTests).Assembly,
			                     typeof(XunitTestingApplicationAttribute).Assembly);
		}

		[Fact]
		void VerifyDevelopment()
		{
			EnvironmentAwareAssemblies.Default.Get("Development")
			              .Get()
			              .Open()
			              .Should()
			              .Equal(typeof(Testing.Environment.Development.HelloWorld).Assembly,
			                     typeof(Testing.Environment.HelloWorld).Assembly,
			                     typeof(EnvironmentAwareAssembliesTests).Assembly,
			                     typeof(XunitTestingApplicationAttribute).Assembly);
		}
	}
}