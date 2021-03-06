﻿using DragonSpark.Compose.Model;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Compose.Extents.Selections
{
	public sealed class Into<TIn, TOut>
	{
		public static Into<TIn, TOut> Default { get; } = new Into<TIn, TOut>();

		Into() {}

		public ITable<TIn, TOut> Table() => Table(x => default);

		public ITable<TIn, TOut> Table(Func<TIn, TOut> select) => Tables<TIn, TOut>.Default.Get(select);

		public ICondition<TIn> Condition(Func<TIn, bool> condition)
			=> new DragonSpark.Model.Selection.Conditions.Condition<TIn>(condition);

		public IAction<TIn> Action(System.Action<TIn> body) => new Model.Action<TIn>(body);
	}
}