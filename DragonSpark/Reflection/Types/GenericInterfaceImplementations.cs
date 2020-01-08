using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericInterfaceImplementations : Store<Type, IConditional<Type, Array<Type>>>
	{
		public static GenericInterfaceImplementations Default { get; } = new GenericInterfaceImplementations();

		GenericInterfaceImplementations() : this(GenericTypeDefinition.Default.Then()) {}

		public GenericInterfaceImplementations(Selector<Type, Type> definition)
			: base(GenericInterfaces.Default.Query()
			                        .GroupMap(definition.Get())
			                        .Select(definition.Use.UnlessCalling(A.Self<Type>())
			                                          .Allows(IsGenericTypeDefinition.Default)
			                                          .Use.UnlessCalling)
			                        .Select(A.ConditionalResult<Type, Array<Type>>)
			                        .Then()
			                        .Value()) {}
	}
}