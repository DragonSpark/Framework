using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Editing;

public interface IModifying<T> : IOperation<Edit<T>> {}