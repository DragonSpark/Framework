using DragonSpark.Compose;
using DragonSpark.Reflection;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Reflection
{
	public class AccountForUnassignedTypeTests
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