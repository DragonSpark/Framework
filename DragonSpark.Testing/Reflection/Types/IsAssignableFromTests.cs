using DragonSpark.Reflection.Types;
using FluentAssertions;
using System.IO;
using Xunit;

namespace DragonSpark.Testing.Reflection.Types
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