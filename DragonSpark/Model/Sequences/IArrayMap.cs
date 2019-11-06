using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Sequences
{
	public interface IArrayMap<in TIn, T> : IConditional<TIn, Array<T>> {}
}