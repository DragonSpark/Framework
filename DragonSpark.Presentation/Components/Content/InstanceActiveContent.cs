using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Content;

sealed class InstanceActiveContent<T> : Deferring<T?>, IActiveContent<T>
{
	public InstanceActiveContent(IActiveContent<T> previous) : base(previous.ToDelegate()) {}
}