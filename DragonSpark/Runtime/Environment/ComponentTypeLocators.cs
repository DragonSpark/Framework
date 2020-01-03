using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace DragonSpark.Runtime.Environment
{
	public interface IComponentType : IAlteration<Type> {}

	sealed class ComponentType : Alteration<Type>, IComponentType
	{
		public ComponentType(IArray<Type, Type> @select) : base(@select.Query().FirstAssigned()) {}
	}

	public interface IComponentTypes : IArray<Type, Type> {}

	sealed class ComponentTypes : ArrayStore<Type, Type>, IComponentTypes, IActivateUsing<ISelect<Type, Array<Type>>>
	{
		public ComponentTypes(ISelect<Type, Array<Type>> source) : base(source) {}
	}

	sealed class ComponentTypeLocators : Select<IReadOnlyList<Type>, IComponentTypes>
	{
		public static ComponentTypeLocators Default { get; } = new ComponentTypeLocators();

		ComponentTypeLocators() : this(IsComponentType.Default.Get) {}

		public ComponentTypeLocators(Func<Type, bool> condition)
			: base(Start.A.Selection<Type>()
			            .As.Sequence.ReadOnly.By.Self.Query()
			            .Where(condition)
			            .Get()
			            .Then()
			            .Select<TypeCandidates>()
			            .Select(x => x.Open()
			                          .Then()
			                          .Sort()
			                          .Out()
			                          .To(Start.An.Extent<ComponentTypes>())
			                   )
			      ) {}
	}

	/*sealed class ComponentTypes : IArray<IReadOnlyList<Type>, Type>
	{
		public static ComponentTypes Default { get; } = new ComponentTypes();

		ComponentTypes() : this(IsComponentType.Default.Get) {}

		readonly Func<Type, bool> _condition;

		public ComponentTypes(Func<Type, bool> condition) => _condition = condition;

		public Array<Type> Get(IReadOnlyList<Type> parameter)
			=> new ComponentTypesSelector(parameter.Where(_condition).Result()).Open().Then().Sort().Out().ToDelegate();
	}*/
}