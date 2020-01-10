using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class TypeSelection<T> : TypeSelection where T : class, IActivateUsing<Assembly>, IArray<Type>
	{
		public static TypeSelection<T> Default { get; } = new TypeSelection<T>();

		TypeSelection() : base(Start.An.Extent<T>().From) {}
	}

	class TypeSelection : ArraySelection<Array<Assembly>, Type>
	{
		public TypeSelection(Func<Assembly, IArray<Type>> select)
			: this(Start.A.Selection<Assembly>()
			            .As.Sequence.Array.By.Self.Get()
			            .Query()
			            .Query(x => x.Select(select) // TODO: hyperlinq?
			                          .SelectMany(y => y.Get().Open())
			                          .ToArray())
			       /*.SelectMany(x => x.Get().Open())*/) {}

		public TypeSelection(Func<Array<Assembly>, Array<Type>> select) : base(select) {}
	}
}