﻿namespace DragonSpark.Model.Selection.Conditions
{
	public interface IConditionAware<in T>
	{
		ICondition<T> Condition { get; }
	}
}