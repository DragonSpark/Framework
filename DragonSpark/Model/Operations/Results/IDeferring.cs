using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Operations.Results;

public interface IDeferring<T> : IResulting<T>, ICondition {}