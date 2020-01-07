using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results
{
	public interface IStore<T> : IMutable<T>, IConditionAware<None> {}
}