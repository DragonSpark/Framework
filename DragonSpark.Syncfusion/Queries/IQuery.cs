using DragonSpark.Model.Operations;

namespace DragonSpark.SyncfusionRendering.Queries;

public interface IQuery<T> : IAltering<Parameter<T>> {}