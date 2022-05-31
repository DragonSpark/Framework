using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Content;

sealed class InstanceActiveContent<T> : Deferring<T?>, IActiveContent<T>
{
	public InstanceActiveContent(IActiveContent<T> previous) : this(previous, UpdateMonitor.Default) {}

	public InstanceActiveContent(IActiveContent<T> previous, IUpdateMonitor refresh) : base(previous.ToDelegate())
		=> Monitor = refresh;

	public IUpdateMonitor Monitor { get; }
}