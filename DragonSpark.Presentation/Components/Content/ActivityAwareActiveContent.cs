using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActivityAwareActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	public ActivityAwareActiveContent(IActiveContent<T> previous, object receiver)
		: this(previous.Refresh, new ActivityAwareResult<T>(previous, receiver)) {}

	public ActivityAwareActiveContent(IRequiresUpdate refresh, IResulting<T?> resulting) : base(resulting)
		=> Refresh = refresh;

	public IRequiresUpdate Refresh { get; }
}