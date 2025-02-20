using System.Linq;
using System.Reflection;
using DragonSpark.Runtime.Environment;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime.Environment;

public sealed class ComponentAssemblyCandidatesTests
{
    [Fact]
    public void Verify()
    {
        var source = new AssemblyName("DragonSpark.Duper.Awesome.Namespace.Application");
        var expected = new[]
        {
            source,
            new("DragonSpark.Duper.Awesome.Namespace"),
            new("DragonSpark.Duper.Awesome"),
            new("DragonSpark.Duper"),
            new("DragonSpark")
        }.Select(x => x.FullName);

        var enumerable = ComponentAssemblyCandidates.Default.Get(source).Select(x => x.FullName);
        enumerable.Should().BeEquivalentTo(expected);
    }
}