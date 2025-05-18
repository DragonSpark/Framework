using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities;

public interface IInput<TIn, T> : IStopAware<In<TIn>, T>;