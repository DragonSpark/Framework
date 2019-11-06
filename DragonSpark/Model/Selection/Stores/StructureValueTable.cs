using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Selection.Stores
{
	sealed class StructureValueTable<TIn, TOut> : DecoratedTable<TIn, TOut>
		where TIn : class
		where TOut : struct
	{
		public StructureValueTable(Func<TIn, TOut> parameter)
			: base(new StructureValueTables<TIn, TOut>(parameter).Get(new ConditionalWeakTable<TIn, Tuple<TOut>>())) {}
	}
}