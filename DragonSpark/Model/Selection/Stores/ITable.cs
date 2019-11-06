using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Selection.Stores
{
	public interface ITable<TIn, TOut> : IConditional<TIn, TOut>, IAssign<TIn, TOut>
	{
		bool Remove(TIn key);
	}
}