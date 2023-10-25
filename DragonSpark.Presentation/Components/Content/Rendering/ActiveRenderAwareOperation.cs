using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public sealed class ActiveRenderAwareOperation : IOperation
{
	readonly IOperation           _previous;
	readonly IResult<RenderState> _state;

	public ActiveRenderAwareOperation(Func<Task> previous, IResult<RenderState> state)
		: this(new Allocated(previous).Then().Structure().Out(), state) {}

	public ActiveRenderAwareOperation(IOperation previous, IResult<RenderState> state)
	{
		_previous = previous;
		_state    = state;
	}

	public ValueTask Get()
	{
		switch (_state.Get())
		{
			case RenderState.Ready:
			case RenderState.Established:
				return _previous.Get();
		}
		return ValueTask.CompletedTask;
	}
}

public sealed class ActiveRenderAwareOperation<T> : IOperation<T>
{
	readonly IOperation<T>        _previous;
	readonly IResult<RenderState> _state;

	public ActiveRenderAwareOperation(Func<T, Task> previous, IResult<RenderState> state)
		: this(new Allocated<T>(previous).Then().Structure().Out(), state) {}

	public ActiveRenderAwareOperation(IOperation<T> previous, IResult<RenderState> state)
	{
		_previous = previous;
		_state    = state;
	}

	public ValueTask Get(T parameter)
	{
		switch (_state.Get())
		{
			case RenderState.Ready:
			case RenderState.Established:
				return _previous.Get(parameter);
		}
		return ValueTask.CompletedTask;
	}
}