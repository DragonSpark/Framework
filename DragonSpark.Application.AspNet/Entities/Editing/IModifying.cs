using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public interface IModifying<T> : IOperation<Edit<T>>;