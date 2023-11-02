using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Server.Requests;

public interface IPolicy : ISelecting<Unique, bool?>;

public interface IPolicy<T> : ISelecting<Request<T>, bool?>;