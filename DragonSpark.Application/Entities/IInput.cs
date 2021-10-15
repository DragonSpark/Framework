using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities;

public interface IInput<TIn, T> : ISelecting<In<TIn>, T> {}