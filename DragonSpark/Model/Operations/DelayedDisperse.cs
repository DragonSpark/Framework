using System;

namespace DragonSpark.Model.Operations;

public class DelayedDisperse<T> : Disperse<T>
{
	public DelayedDisperse(IOperation<T> previous) : this(previous, TimeSpan.FromSeconds(1)) {}

	public DelayedDisperse(IOperation<T> previous, TimeSpan delay) : base(new Delay<T>(previous, delay)) {}
}