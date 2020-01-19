﻿using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Linq;

namespace DragonSpark.Compose.Model
{
	public class ConditionSelector<T> : Selector<T, bool>
	{
		public static implicit operator Func<T, bool>(ConditionSelector<T> instance) => instance.Get().Get;

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
}