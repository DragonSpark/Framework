using DragonSpark.Compose;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Reflection.Types;

sealed class GenericInterfaceImplementations : Store<Type, IConditional<Type, Array<Type>>>
{
	public static GenericInterfaceImplementations Default { get; } = new GenericInterfaceImplementations();

	GenericInterfaceImplementations() : this(GenericTypeDefinition.Default.Then()) {}

	public GenericInterfaceImplementations(Selector<Type, Type> definition)
		: base(GenericInterfaces.Default.Then()
		                        .GroupMap(definition.Get().Get)
		                        .Select(definition.Unless.Input.Is(IsGenericTypeDefinition.Default)
		                                          .ThenUse(A.Self<Type>())
		                                          .Unless.UsingWhen)
		                        .Select(A.ConditionalResult<Type, Array<Type>>)
		                        .Then()
		                        .Value()) {}
}