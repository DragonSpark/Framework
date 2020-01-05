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
		public ComponentType(IArray<Type, Type> select)
			: base(select.Query()
			             .FirstAssigned()
			             .Then()
			             //.Introduce()
			             // TODO:
			             .Ensure.Assigned.Exit.OrThrow(LocateGuardMessage.Default)) {}
	}

	/*sealed class Select : ISelect<Type, Type>
	{
		readonly IArray<Type, Type> _select;

		public Select(IArray<Type, Type> select) => _select = select;

		public Type Get(Type parameter)
		{

			var type = _select.Query()
			                  .FirstAssigned()
			                  .Get(parameter);

			var other = Start.A.Selection.Of.System.Type.By.Self.Then()
			                 .Ensure.Assigned.Exit.OrThrow(LocateGuardMessage.Default)
			                 .Get();

			return type;
		}
	}*/

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
}