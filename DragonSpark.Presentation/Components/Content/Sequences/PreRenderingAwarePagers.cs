namespace DragonSpark.Presentation.Components.Content.Sequences;

/*sealed class PreRenderingAwarePagers<T> : ISelect<PreRenderingAwarePagersInput<T>, IPaging<T>>
{
	readonly IMemoryCache       _memory;
	readonly CurrentRenderState _state;

	public PreRenderingAwarePagers(IMemoryCache memory, CurrentRenderState state)
	{
		_memory = memory;
		_state  = state;
	}

	public IPaging<T> Get(PreRenderingAwarePagersInput<T> parameter)
	{
		var (previous, formatter) = parameter;
		var content = new MemoryAwarePagingContent<T>(previous, _state, new(_memory));
		var result  = new MemoryAwarePaging<T>(formatter, content);
		return result;
	}
}*/