using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Results
{
	public interface IStore<T> : IMutable<T>, IConditionAware<None> {}
}