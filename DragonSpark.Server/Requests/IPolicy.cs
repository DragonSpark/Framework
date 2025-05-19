using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Requests;

public interface IPolicy : IStopAware<Unique, bool?>;

public interface IPolicy<T> : ISelecting<Request<T>, bool?>;