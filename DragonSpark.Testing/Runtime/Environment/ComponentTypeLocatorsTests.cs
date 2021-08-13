using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Reflection.Selection;
using DragonSpark.Runtime.Environment;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Runtime.Environment
{
	public sealed class ComponentTypeLocatorsTests
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
		public void Verify()
		{
			SortSelector<Type>.Default.Get(typeof(First)).Should().Be(-10);

			var types = ComponentTypeLocators.Default.Get(NestedTypes<ComponentTypeLocatorsTests>.Default.Get().Open())
			                                 .Get(typeof(IComponent))
			                                 .Open();
			types.Should().HaveCount(4);
			types.Should().BeEquivalentTo(new []{typeof(First), typeof(Subject), typeof(AnotherSubject), typeof(Last)});
			types.First().Should().Be(typeof(First));
			types.Last().Should().Be(typeof(Last));
		}
	}
}