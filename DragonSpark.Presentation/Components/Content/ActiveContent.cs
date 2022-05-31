using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

sealed class ActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	public ActiveContent(Func<ValueTask<T?>> content) : this(content, new Variable<T>(), new Variable<int>()) {}

	public ActiveContent(Func<ValueTask<T?>> content, IMutable<T?> store, IMutable<int> counts)
		: this(content.Start().Out(), new VisitedAwareVariable<T?>(store, counts), counts) {}

	public ActiveContent(IResulting<T?> result, IMutationAware<T?> store, IMutable<int> counts)
		: this(new Storing<T?>(store, result), counts) {}

	public ActiveContent(IResulting<T?> result, IMutable<int> counts)
		: this(result, new RefreshActiveContent<T?>(result, counts)) {}

	public ActiveContent(IResulting<T?> result, IUpdateMonitor monitor) : base(result) => Monitor = monitor;

	public IUpdateMonitor Monitor { get; }
}