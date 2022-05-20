using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results;

public interface IMutationAware<T> : IMutable<T>, IConditionAware {}