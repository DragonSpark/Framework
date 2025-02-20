using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Runtime.Environment;
using DragonSpark.Testing.Environment;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime.Environment;

public sealed class EnvironmentAwareAssembliesTests
{
	[Fact]
	public void Verify()
	{
		EnvironmentAwareAssemblies.Default.Get(new(null, "Production"))
		                          .Open()
		                          .Should()
		                          .Equal(typeof(HelloWorld).Assembly,
		                                 typeof(EnvironmentAwareAssembliesTests).Assembly,
		                                 typeof(XunitTestingApplicationAttribute).Assembly);
	}

    [Fact]
    public void VerifyPlatform()
    {
        EnvironmentAwareAssemblies.Default.Get(new("Platform", "Development"))
                                  .Open()
                                  .Should()
                                  .Equal(typeof(Testing.Environment.Platform.Development.HelloWorld).Assembly,
                                         typeof(Testing.Environment.Development.HelloWorld).Assembly,
                                         typeof(HelloWorld).Assembly,
                                         typeof(EnvironmentAwareAssembliesTests).Assembly,
                                         typeof(XunitTestingApplicationAttribute).Assembly);
    }

	[Fact]
	public void VerifyDevelopment()
	{
		EnvironmentAwareAssemblies.Default.Get(new(null, "Development"))
		                          .Open()
		                          .Should()
		                          .Equal(typeof(Testing.Environment.Development.HelloWorld).Assembly,
		                                 typeof(HelloWorld).Assembly,
		                                 typeof(EnvironmentAwareAssembliesTests).Assembly,
		                                 typeof(XunitTestingApplicationAttribute).Assembly);
	}
}