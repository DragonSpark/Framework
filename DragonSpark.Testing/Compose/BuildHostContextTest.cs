using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Xunit;

namespace DragonSpark.Testing.Compose;

[TestSubject(typeof(BuildHostContext))]
public sealed class BuildHostContextTest
{
	[Fact]
	public async Task Verify()
	{
		StringBuilder builder = new();
		_ = await Start.A.Host()
		               .Configure((IServiceCollection _) => builder.Append("a"))
		               .Configure((IServiceCollection _) => builder.Append("b"))
		               .Allocate(new HostBuilder());
		builder.ToString().Should().Be("ab");
	}
}