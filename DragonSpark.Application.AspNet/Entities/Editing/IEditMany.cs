using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public interface IEditMany<in TIn, T> : ISelecting<TIn, ManyEdit<T>>;