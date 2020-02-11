using DragonSpark.Runtime.Environment;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Runtime.Environment
{
	public sealed class ComponentAssemblyCandidatesTests
	{
		[Fact]
		void Verify()
		{
			ComponentAssemblyCandidates.Default
			                           .Get(new AssemblyName("DragonSpark.Duper.Awesome.Namespace.Application"))
			                           .Should()
			                           .BeEquivalentTo(new AssemblyName("DragonSpark.Duper.Awesome.Namespace.Application"),
			                                           new AssemblyName("DragonSpark.Duper.Awesome.Namespace"),
			                                           new AssemblyName("DragonSpark.Duper.Awesome"),
			                                           new AssemblyName("DragonSpark.Duper"),
			                                           new AssemblyName("DragonSpark"));
		}
	}
}