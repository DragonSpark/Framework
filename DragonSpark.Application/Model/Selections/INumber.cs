using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Model.Selections;

public interface INumber<T> : IStopAware<uint, T>;