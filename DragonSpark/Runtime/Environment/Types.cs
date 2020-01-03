using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class TypeSelection<T> : TypeSelection where T : class, IActivateUsing<Assembly>, IArray<Type>
	{
		public static TypeSelection<T> Default { get; } = new TypeSelection<T>();

		TypeSelection() : base(Start.An.Extent<T>().From) {}
	}

	class TypeSelection : ArraySelection<IReadOnlyList<Assembly>, Type>
	{
		public TypeSelection(Func<Assembly, IArray<Type>> select)
			: this(Start.A.Selection<Assembly>()
			            .As.Sequence.ReadOnly.By.Self.Query()
			            .Select(select)
			            .SelectMany(x => x.Get().Open())
			            .Selector()) {}

		public TypeSelection(Func<IReadOnlyList<Assembly>, Array<Type>> @select) : base(@select) {}
	}
}