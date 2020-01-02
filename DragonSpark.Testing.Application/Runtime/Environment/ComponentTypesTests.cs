using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Reflection.Selection;
using DragonSpark.Runtime.Environment;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class ComponentTypesTests
	{
		public interface IComponent {}

		sealed class Subject : IComponent {}

		sealed class AnotherSubject : IComponent {}

		[Sort(-10)]
		sealed class First : IComponent {}

		sealed class Last : IComponent, ISortAware
		{
			public int Get() => 100;
		}

		[Fact]
		void Verify()
		{
			SortSelector<Type>.Default.Get(typeof(First)).Should().Be(-10);

			var types = new ComponentTypesDefinition(NestedTypes<ComponentTypesTests>.Default)
			            .Get(typeof(IComponent))
			            .Open();
			types.Should().HaveCount(4);
			types.Should().BeEquivalentTo(typeof(First), typeof(Subject), typeof(AnotherSubject), typeof(Last));
			types.First().Should().Be(typeof(First));
			types.Last().Should().Be(typeof(Last));
		}
	}
}