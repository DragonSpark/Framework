using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Model.Selections;

public interface INumberWithStop<T> : IStopAware<ulong, T>;