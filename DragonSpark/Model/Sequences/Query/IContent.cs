using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query
{
	public interface IContent<TIn, TOut> : ISelect<Store<TIn>, Store<TOut>> {}
}