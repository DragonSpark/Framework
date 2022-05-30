using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;
using System;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActivityAwareActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	public ActivityAwareActiveContent(IActiveContent<T> previous, object receiver)
		: this(previous.Refresh, new ActivityAwareResult<T>(previous, receiver)) {}

	public ActivityAwareActiveContent(IOperation<Action> refresh, IResulting<T?> resulting) : base(resulting)
		=> Refresh = refresh;

	public IOperation<Action> Refresh { get; }
}