using FluentAssertions;
using DragonSpark.Reflection;
using Xunit;

namespace DragonSpark.Testing.Application.Reflection
{
	public class ObjectsTests
	{
		[Fact]
		public void AccountForNullable()
		{
			AccountForUnassignedType.Default.Get(typeof(int?))
			                        .Should()
			                        .Be<int>();

			AccountForUnassignedType.Default.Get(GetType())
			                        .Should()
			                        .Be(GetType());
		}
	}
}