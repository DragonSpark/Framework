using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Selection;
using DragonSpark.Runtime.Activation;
using System;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	public sealed class Types : SystemStore<Array<Type>>, IArray<Type>
	{
		public static Types Default { get; } = new Types();

		Types() : base(Types<PublicAssemblyTypes>.Default) {}
	}

	public sealed class Types<T> : ArrayStore<Type> where T : class, IActivateUsing<Assembly>, IArray<Type>
	{
		public static Types<T> Default { get; } = new Types<T>();

		Types() : base(Assemblies.Default.Query()
		                         .Select(Start.An.Extent<T>().From)
		                         .SelectMany(x => x.Get().Open())
		                         .Selector()) {}
	}
}