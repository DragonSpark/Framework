using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericInterfaceImplementations : Store<Type, IConditional<Type, Array<Type>>>
	{
		public static GenericInterfaceImplementations Default { get; } = new GenericInterfaceImplementations();

		GenericInterfaceImplementations() : this(GenericTypeDefinition.Default) {}

		public GenericInterfaceImplementations(ISelect<Type, Type> definition)
			: base(GenericInterfaces.Default.Query()
			                        .GroupMap(definition.ToDelegate())
			                        .Select(definition.Unless(IsGenericTypeDefinition.Default, A.Self<Type>()).Select)
			                        .Get) {}
	}
}