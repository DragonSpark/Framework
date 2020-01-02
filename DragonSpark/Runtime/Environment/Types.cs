using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using System;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class Types<T> : ArrayStore<Type> where T : class, IActivateUsing<Assembly>, IArray<Type>
	{
		public static Types<T> Default { get; } = new Types<T>();

		Types() : base(Start.An.Instance(Assemblies.Default)
		                    .Query()
		                    .Select(Start.An.Extent<T>().From)
		                    .SelectMany(x => x.Get().Open())
		                    .Selector()) {}
	}

	sealed class TypeSelection<T> : TypeSelection where T : class, IActivateUsing<Assembly>, IArray<Type>
	{
		public static TypeSelection<T> Default { get; } = new TypeSelection<T>();

		TypeSelection() : base(Start.An.Extent<T>().From) {}
	}

	class TypeSelection : ArraySelection<Array<Assembly>, Type>
	{
		public TypeSelection(Func<Assembly, IArray<Type>> select)
			: this(Start.A.Selection<Assembly>()
			            .As.Sequence.Array.By.Self.Query()
			            .Select(select)
			            .SelectMany(x => x.Get().Open())
			            .Selector()) {}

		public TypeSelection(Func<Array<Assembly>, Array<Type>> @select) : base(@select) {}
	}
}