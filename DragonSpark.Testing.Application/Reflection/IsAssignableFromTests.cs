using System.IO;
using FluentAssertions;
using DragonSpark.Reflection.Types;
using Xunit;

namespace DragonSpark.Testing.Application.Reflection
{
	public class IsAssignableFromTests
	{
		[Fact]
		public void Verify()
		{
			IsAssignableFrom<Stream>.Default
			                        .Get(typeof(MemoryStream))
			                        .Should()
			                        .BeTrue();
		}
	}
}