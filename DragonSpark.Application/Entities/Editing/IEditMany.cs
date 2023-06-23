using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Editing;

public interface IEditMany<in TIn, T> : ISelecting<TIn, ManyEdit<T>> {}