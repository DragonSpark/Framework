using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Runtime.Environment
{
	public sealed class PrimaryAssemblyDetailsTests
	{
		[Fact]
		public void Verify()
		{
			var details  = PrimaryAssemblyDetails.Default.Get();
			var assembly = Assembly.GetExecutingAssembly();
			details.Version.Should().BeEquivalentTo(assembly.GetName().Version);
			details.Title.Should().NotBeNull().And.Be(AssemblyTitle.Default.Get(assembly));
		}
	}
}