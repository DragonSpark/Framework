using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model.Sequences
{
	public class ArrayMap<_, T> : Select<_, Array<T>>, IArrayMap<_, T>
	{
		public ArrayMap(IConditional<_, Array<T>> source) : this(source.Condition, source.Get) {}

		public ArrayMap(ICondition<_> condition, Func<_, Array<T>> source) : base(source) => Condition = condition;

		public ICondition<_> Condition { get; }
	}
}