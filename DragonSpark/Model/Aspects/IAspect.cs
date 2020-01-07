using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Model.Aspects
{
	public interface IAspect<TIn, TOut> : IAlteration<ISelect<TIn, TOut>> {}
}