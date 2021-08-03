using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Selection
{
	public class NestedConditionSelector : ConditionSelector<None>
	{
		public static implicit operator Func<bool>(NestedConditionSelector instance) => instance.Get().Get;

		public NestedConditionSelector(ISelect<None, bool> subject) : base(subject) {}
	}

	public class NestedConditionSelector<_, T> : SelectionSelector<_, T, bool>
	{
		public NestedConditionSelector(ISelect<_, ISelect<T, bool>> subject) : base(subject) {}
	}
}