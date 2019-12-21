using DragonSpark.Model.Selection.Conditions;
using System;
using System.Linq;

namespace DragonSpark.Model.Selection.Adapters
{
	public class ConditionSelector<T> : Selector<T, bool>
	{
		public ConditionSelector(ISelect<T, bool> subject) : base(subject) {}

		public ConditionSelector<T> Or(params Func<T, bool>[] others)
			=> Or(others.Select(x => new Condition<T>(x)).Open<ISelect<T, bool>>());

		public ConditionSelector<T> Or(params ISelect<T, bool>[] others)
			=> new ConditionSelector<T>(new AnyCondition<T>(others.Prepend(Get()).Open()));

		public ConditionSelector<T> And(params Func<T, bool>[] others)
			=> And(others.Select(x => new Condition<T>(x)).Open<ISelect<T, bool>>());

		public ConditionSelector<T> And(params ISelect<T, bool>[] others)
			=> new ConditionSelector<T>(new AllCondition<T>(others.Prepend(Get()).Open()));

		public ConditionSelector<T> Inverse()
			=> new ConditionSelector<T>(InverseConditions<T>.Default.Get(new Condition<T>(Get().Get)));
	}

	public class ConditionSelector<_, T> : SelectionSelector<_, T, bool>
	{
		public ConditionSelector(ISelect<_, ISelect<T, bool>> subject) : base(subject) {}
	}
}