using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Content;

sealed class InstanceActiveContent<T> : Deferring<T?>, IActiveContent<T>
{
	public InstanceActiveContent(IActiveContent<T> previous) : this(previous, RequiresUpdate.Default) {}

	public InstanceActiveContent(IActiveContent<T> previous, IRequiresUpdate refresh) : base(previous.ToDelegate())
		=> Refresh = refresh;

	public IRequiresUpdate Refresh { get; }
}