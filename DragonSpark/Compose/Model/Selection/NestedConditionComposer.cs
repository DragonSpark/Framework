using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Selection;

public class NestedConditionComposer : ConditionComposer<None>
{
	public static implicit operator Func<bool>(NestedConditionComposer instance) => instance.Get().Get;

	public NestedConditionComposer(ISelect<None, bool> subject) : base(subject) {}
}

public class NestedConditionComposer<_, T> : SelectionComposer<_, T, bool>
{
	public NestedConditionComposer(ISelect<_, ISelect<T, bool>> subject) : base(subject) {}
}