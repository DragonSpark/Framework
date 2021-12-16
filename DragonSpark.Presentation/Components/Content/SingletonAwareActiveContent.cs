using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Content;

sealed class SingletonAwareActiveContent<T> : Deferring<T?>, IActiveContent<T>
{
	public SingletonAwareActiveContent(IActiveContent<T> previous) : base(previous.ToDelegate()) {}
}