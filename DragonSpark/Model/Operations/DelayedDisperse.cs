using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Operations;

public class DelayedDisperse : DelayedDisperse<None>
{
	public DelayedDisperse(IOperation previous) : this(previous, TimeSpan.FromSeconds(1)) {}

	protected DelayedDisperse(IOperation previous, TimeSpan delay) : base(previous.Then().Accept().Out(), delay) {}
}

public class DelayedDisperse<T> : Disperse<T>
{
	protected DelayedDisperse(IOperation<T> previous) : this(previous, TimeSpan.FromSeconds(1)) {}

	protected DelayedDisperse(IOperation<T> previous, TimeSpan delay) : base(new Delay<T>(previous, delay)) {}
}