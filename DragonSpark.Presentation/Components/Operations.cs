using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Presentation.Components
{
	sealed class Operations : ISelect<Type, Array<Func<ComponentBase, IOperation>>>
	{
		public static Operations Default { get; } = new Operations();

		Operations() : this(Delegates.Default) {}

		readonly IGeneric<IArray<Func<ComponentBase, IOperation>>> _generic;

		public Operations(IGeneric<IArray<Func<ComponentBase, IOperation>>> generic) => _generic = generic;

		public Array<Func<ComponentBase, IOperation>> Get(Type parameter) => _generic.Get(parameter)().Get();
	}
}