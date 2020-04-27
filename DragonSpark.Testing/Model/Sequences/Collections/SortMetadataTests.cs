using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Collections;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Collections
{
	public class SortMetadataTests
	{
		[Sort(200)]
		sealed class Subject {}

		[Fact]
		public void Verify()
		{
			SortMetadata<Subject>.Default.Get(new Subject()).Should().Be(200);
		}

		[Fact]
		public void VerifyMetadata()
		{
			SortMetadata<TypeInfo>.Default.Get(A.Metadata<Subject>()).Should().Be(200);
		}
	}
}