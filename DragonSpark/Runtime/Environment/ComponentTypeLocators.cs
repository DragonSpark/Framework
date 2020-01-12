using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using DragonSpark.Text;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Runtime.Environment
{
	public interface IComponentType : IAlteration<Type> {}

	sealed class ComponentType : Alteration<Type>, IComponentType
	{
		public ComponentType(IArray<Type, Type> select)
			: base(select.Then()
			             .FirstAssigned()
			             .Ensure.Output.IsAssigned.Otherwise.Throw(LocateGuardMessage.Default)) {}
	}

	sealed class LocateComponentMessage<T> : Message<Type>
	{
		public static LocateComponentMessage<T> Default { get; } = new LocateComponentMessage<T>();

		LocateComponentMessage() :
			base(x => $"A request was made to locate a type of {A.Type<T>()}.  Its implementation type of {x} was located, but it could not be activated.") {}
	}

	public interface IComponentTypes : IArray<Type, Type> {}

	sealed class ComponentTypes : ArrayStore<Type, Type>, IComponentTypes, IActivateUsing<ISelect<Type, Array<Type>>>
	{
		public ComponentTypes(ISelect<Type, Array<Type>> source) : base(source) {}
	}

	sealed class ComponentTypeLocators : Select<Array<Type>, IComponentTypes>
	{
		public static ComponentTypeLocators Default { get; } = new ComponentTypeLocators();

		ComponentTypeLocators() : this(IsComponentType.Default.Get) {}

		public ComponentTypeLocators(Func<Type, bool> condition)
			: base(Start.A.Selection<Type>()
			            .As.Sequence.Array.By.Self.Open()
			            .Select(x => x.Where(condition).ToArray().Result())
			            .StoredActivation<TypeCandidates>()
			            .Select(x => x.Open()
			                          .Then()
			                          .Sort()
			                          .Result()
			                          .Get()
			                          .To(Start.An.Extent<ComponentTypes>())
			                   )
			      ) {}
	}
}