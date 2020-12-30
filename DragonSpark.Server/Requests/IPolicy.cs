using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Requests
{
	public interface IPolicy : ISelecting<Query, bool?> {}
}