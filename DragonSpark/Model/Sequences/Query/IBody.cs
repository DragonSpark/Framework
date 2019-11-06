using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query
{
	public interface IBody<T> : IBody<T, T> {}

	public interface IBody<TIn, TOut> : ISelect<ArrayView<TIn>, ArrayView<TOut>> {}
}