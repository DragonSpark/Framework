namespace DragonSpark.Application.Compose.Store
{
	delegate TOut Get<TIn, out TOut>((TIn Parameter, object Key) parameter);
}