using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.SyncfusionRendering.Queries;

public interface IQuery<T> : IAltering<Parameter<T>>;