using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Requests
{
	public interface IPolicy : ISelecting<Query, bool?> {}

	public interface IPolicy<T> : ISelecting<Request<T>, bool?> {}

}