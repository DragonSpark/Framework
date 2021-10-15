namespace DragonSpark.Model.Selection;

sealed class Default<T> : FixedResult<T, T>
{
	public static Default<T> Instance { get; } = new Default<T>();

	Default() : base(default!) {}
}

sealed class Default<TIn, TOut> : FixedResult<TIn, TOut>
{
	public static ISelect<TIn, TOut> Instance { get; } = new Default<TIn, TOut>();

	Default() : base(default!) {}
}