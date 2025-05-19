using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public interface IEditMany<TIn, T> : IStopAware<TIn, ManyEdit<T>>;