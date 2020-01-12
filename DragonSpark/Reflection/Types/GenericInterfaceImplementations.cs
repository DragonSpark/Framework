using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using System;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericInterfaceImplementations : Store<Type, IConditional<Type, Array<Type>>>
	{
		public static GenericInterfaceImplementations Default { get; } = new GenericInterfaceImplementations();

		GenericInterfaceImplementations() : this(GenericTypeDefinition.Default.Then()) {}

		public GenericInterfaceImplementations(Selector<Type, Type> definition)
			: base(GenericInterfaces.Default.Select(new Context(new GroupMap<Type, Type>(definition.Get().Get)))
			                        .Select(definition.Unless.Input.Is(IsGenericTypeDefinition.Default)
			                                          .ThenUse(A.Self<Type>())
			                                          .Unless.UsingWhen)
			                        .Select(A.ConditionalResult<Type, Array<Type>>)
			                        .Then()
			                        .Value()) {}

		// TODO: simplify

		sealed class Context : ISelect<Array<Type>, IArrayMap<Type, Type>>
		{
			readonly ISelect<Type[], IArrayMap<Type, Type>> _reduce;

			public Context(ISelect<Type[], IArrayMap<Type, Type>> reduce) => _reduce = reduce;

			public IArrayMap<Type, Type> Get(Array<Type> parameter) => _reduce.Get(parameter.Open());
		}
	}
}