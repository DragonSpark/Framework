using System.Reflection;
using FluentAssertions;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Collections;
using DragonSpark.Reflection.Types;
using Xunit;

namespace DragonSpark.Testing.Application.Reflection
{
	public class InnerTypeTests
	{
		[Fact]
		public void Coverage()
		{
			InnerType.Default.Get(GetType())
			         .Should()
			         .BeNull();
		}

		[Fact]
		void Verify()
		{
			InnerType.Default.Get(Type<Always<TypeInfo>>.Instance).Should().Be<TypeInfo>();
		}
	}
}