using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Presentation.Components.Content;

sealed class InstanceActiveContent<T> : Deferring<T?>, IActiveContent<T>
{
	public IOperation<Action> Refresh { get; }

	public InstanceActiveContent(IActiveContent<T> previous) : this(previous, EmptyOperation<Action>.Default) {}

	public InstanceActiveContent(IActiveContent<T> previous, IOperation<Action> refresh) : base(previous.ToDelegate())
		=> Refresh = refresh;
}