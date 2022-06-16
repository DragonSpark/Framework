namespace DragonSpark.Presentation.Components.Content.Sequences;

/*sealed class PreRenderingAwareAny<T> : ISelect<PreRenderingAwareAnyInput<T>, IDepending<IQueries<T>>>
{
	readonly IMemoryCache       _memory;
	readonly CurrentRenderState _state;

	public PreRenderingAwareAny(IMemoryCache memory, CurrentRenderState state)
	{
		_memory = memory;
		_state = state;
	}

	public IDepending<IQueries<T>> Get(PreRenderingAwareAnyInput<T> parameter)
	{
		var (previous, key) = parameter;
		var content = new MemoryAwareAnyContent<T>(previous, _state, new(_memory));
		var result  = new MemoryAwareAny<T>(key, content);
		return result;
	}
}*/