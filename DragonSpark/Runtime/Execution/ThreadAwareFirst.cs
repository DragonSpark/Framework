using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime.Execution;

public class ThreadAwareFirst : FirstBase
{
	public ThreadAwareFirst() : base(new SafeCounter()) {}
}

public sealed class ThreadAwareFirst<T> : Condition<T> where T : notnull
{
	public ThreadAwareFirst() : base(Start.A.Selection<T>()
	                           .AndOf<ThreadAwareFirst>()
	                           .By.Activation()
	                           .Get()
	                           .ToTable()
	                           .Select(ConditionSelector.Default)) {}
}