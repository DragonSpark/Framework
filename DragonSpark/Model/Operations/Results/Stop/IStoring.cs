using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Operations.Results.Stop;

public interface IStoring<T> : IStopAware<T>, ICondition;