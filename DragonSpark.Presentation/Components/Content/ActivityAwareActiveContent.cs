using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActivityAwareActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	public ActivityAwareActiveContent(IActiveContent<T> previous, object receiver)
		: this(previous.Monitor, new ActivityAwareResult<T>(previous, receiver)) {}

	public ActivityAwareActiveContent(IUpdateMonitor refresh, IResulting<T?> resulting) : base(resulting)
		=> Monitor = refresh;

	public IUpdateMonitor Monitor { get; }
}