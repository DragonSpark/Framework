using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentTypeLocators : Select<Array<Type>, IComponentTypes>
	{
		public static ComponentTypeLocators Default { get; } = new ComponentTypeLocators();

		ComponentTypeLocators() : this(IsComponentType.Default.Get) {}

		public ComponentTypeLocators(Func<Type, bool> condition)
			: base(Start.A.Selection<Type>()
			            .As.Sequence.Array.By.Self.Open()
			            .Select(x => x.AsValueEnumerable().Where(condition).ToArray().Result())
			            .StoredActivation<TypeCandidates>()
			            .Select(x => x.Open().Then().Sort().Result().Get())
			            .Select(x => new ComponentTypes(x))) {}
	}
}