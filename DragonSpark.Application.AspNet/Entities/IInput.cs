using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities;

public interface IInput<TIn, T> : ISelecting<In<TIn>, T>;