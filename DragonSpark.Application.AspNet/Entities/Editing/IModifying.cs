using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public interface IModifying<T> : IStopAware<Edit<T>>;