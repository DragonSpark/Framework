using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Operations;

public interface IDeferring<T> : IResulting<T>, ICondition {}