using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Selection.Stores
{
	public class ReferenceValueTable<TIn, TOut> : DecoratedTable<TIn, TOut> where TIn : class
	                                                                        where TOut : class
	{
		[UsedImplicitly]
		public ReferenceValueTable() : this(_ => default!) {}

		public ReferenceValueTable(Func<TIn, TOut> parameter)
			: base(new ReferenceValueTables<TIn, TOut>(parameter).Get(new ConditionalWeakTable<TIn, TOut>())) {}
	}
}